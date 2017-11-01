using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Rendering;
using AvaloniaEdit.Snippets;
using AvalonStudio.Controls.Standard.CodeEditor.Snippets;
using AvalonStudio.CodeEditor;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TextEditor.Rendering;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using AvalonStudio.GlobalSettings;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class CodeEditor : AvaloniaEdit.TextEditor
    {
        private static List<UnsavedFile> _unsavedFiles;

        public static List<UnsavedFile> UnsavedFiles
        {
            get
            {
                if (_unsavedFiles == null)
                {
                    _unsavedFiles = new List<UnsavedFile>();
                }

                return _unsavedFiles;
            }
        }

        private SnippetManager _snippetManager;
        private InsertionContext _currentSnippetContext;
        private bool _suppressIsDirtyNotifications = false;

        public IntellisenseViewModel Intellisense => _intellisense;

        private IntellisenseManager _intellisenseManager;
        private Intellisense _intellisenseControl;
        private IntellisenseViewModel _intellisense;

        private CompletionAssistantView _completionAssistantControl;
        private CompletionAssistantViewModel _completionAssistant;

        private SelectedDebugLineBackgroundRenderer _selectedDebugLineBackgroundRenderer;

        private SelectedWordBackgroundRenderer _selectedWordBackgroundRenderer;

        private ColumnLimitBackgroundRenderer _columnLimitBackgroundRenderer;

        private TextMarkerService _diagnosticMarkersRenderer;

        private TextColoringTransformer _textColorizer;

        private ScopeLineBackgroundRenderer _scopeLineBackgroundRenderer;

        public event EventHandler<TooltipDataRequestEventArgs> RequestTooltipContent;

        private bool _isLoaded = false;

        private int _lastLine = -1;

        private bool _textEntering;
        private readonly IShell _shell;
        private Subject<bool> _analysisTriggerEvents = new Subject<bool>();
        private readonly JobRunner _codeAnalysisRunner;
        private CancellationTokenSource _cancellationSource;
        private CodeEditorToolTip _toolTip;
        private LineNumberMargin _lineNumberMargin;
        private BreakPointMargin _breakpointMargin;
        private SelectedLineBackgroundRenderer _selectedLineBackgroundRenderer;

        /// <summary>
        ///     Write lock must be held before calling this.
        /// </summary>
        public void TriggerCodeAnalysis()
        {
            _analysisTriggerEvents.OnNext(true);
        }

        public ILanguageService LanguageService { get; set; }

        public CodeEditor()
        {
            _codeAnalysisRunner = new JobRunner(1);

            _shell = IoC.Get<IShell>();

            _snippetManager = IoC.Get<SnippetManager>();

            _lineNumberMargin = new LineNumberMargin(this);

            _breakpointMargin = new BreakPointMargin(this, IoC.Get<IDebugManager2>().Breakpoints);

            _selectedLineBackgroundRenderer = new SelectedLineBackgroundRenderer(this);

            _selectedWordBackgroundRenderer = new SelectedWordBackgroundRenderer();

            _columnLimitBackgroundRenderer = new ColumnLimitBackgroundRenderer();

            _selectedDebugLineBackgroundRenderer = new SelectedDebugLineBackgroundRenderer();

            TextArea.TextView.Margin = new Thickness(10, 0, 0, 0);

            TextArea.TextView.BackgroundRenderers.Add(_selectedDebugLineBackgroundRenderer);
            TextArea.TextView.LineTransformers.Add(_selectedDebugLineBackgroundRenderer);

            TextArea.SelectionBrush = Brush.Parse("#AA569CD6");
            TextArea.SelectionCornerRadius = 0;

            this.GetObservable(LineNumbersVisibleProperty).Subscribe(s =>
            {
                if (s)
                {
                    TextArea.LeftMargins.Add(_lineNumberMargin);
                }
                else
                {
                    TextArea.LeftMargins.Remove(_lineNumberMargin);
                }
            });

            this.GetObservable(ShowBreakpointsProperty).Subscribe(s =>
            {
                if (s)
                {
                    TextArea.LeftMargins.Insert(0, _breakpointMargin);
                }
                else
                {
                    TextArea.LeftMargins.Remove(_breakpointMargin);
                }
            });

            this.GetObservable(HighlightSelectedWordProperty).Subscribe(s =>
            {
                if (s)
                {
                    TextArea.TextView.BackgroundRenderers.Add(_selectedWordBackgroundRenderer);
                }
                else
                {
                    TextArea.TextView.BackgroundRenderers.Remove(_selectedWordBackgroundRenderer);
                }
            });

            this.GetObservable(HighlightSelectedLineProperty).Subscribe(s =>
            {
                if (s)
                {
                    TextArea.TextView.BackgroundRenderers.Insert(0, _selectedLineBackgroundRenderer);
                }
                else
                {
                    TextArea.TextView.BackgroundRenderers.Remove(_selectedLineBackgroundRenderer);
                }
            });

            this.GetObservable(ShowColumnLimitProperty).Subscribe(s =>
            {
                if (s)
                {
                    TextArea.TextView.BackgroundRenderers.Add(_columnLimitBackgroundRenderer);
                }
                else
                {
                    TextArea.TextView.BackgroundRenderers.Remove(_columnLimitBackgroundRenderer);
                }
            });

            this.GetObservable(ColumnLimitProperty).Subscribe(limit =>
            {
                _columnLimitBackgroundRenderer.Column = limit;
                this.TextArea.TextView.InvalidateLayer(KnownLayer.Background);
            });

            this.GetObservable(ColorSchemeProperty).Subscribe(colorScheme =>
            {
                if (colorScheme != null)
                {
                    Background = colorScheme.Background;
                    Foreground = colorScheme.Text;

                    _lineNumberMargin.Background = colorScheme.BackgroundAccent;
                    if (_textColorizer != null)
                    {
                        _textColorizer.ColorScheme = colorScheme;
                    }
                    
                    TextArea.TextView.InvalidateLayer(KnownLayer.Background);

                    TriggerCodeAnalysis();
                }
            });

            Options = new AvaloniaEdit.TextEditorOptions
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4
            };

            BackgroundRenderersProperty.Changed.Subscribe(s =>
            {
                if (s.Sender == this)
                {
                    if (s.OldValue != null)
                    {
                        foreach (var renderer in (ObservableCollection<IBackgroundRenderer>)s.OldValue)
                        {
                            TextArea.TextView.BackgroundRenderers.Remove(renderer);
                        }
                    }

                    if (s.NewValue != null)
                    {
                        foreach (var renderer in (ObservableCollection<IBackgroundRenderer>)s.NewValue)
                        {
                            TextArea.TextView.BackgroundRenderers.Add(renderer);
                        }
                    }
                }
            });

            DocumentLineTransformersProperty.Changed.Subscribe(s =>
            {
                if (s.Sender == this)
                {
                    if (s.OldValue != null)
                    {
                        foreach (var renderer in (ObservableCollection<IVisualLineTransformer>)s.OldValue)
                        {
                            TextArea.TextView.LineTransformers.Remove(renderer);
                        }
                    }

                    if (s.NewValue != null)
                    {
                        foreach (var renderer in (ObservableCollection<IVisualLineTransformer>)s.NewValue)
                        {
                            TextArea.TextView.LineTransformers.Add(renderer);
                        }
                    }
                }
            });

            this.GetObservable(CaretOffsetProperty).Subscribe(s =>
            {
                if (Document?.TextLength > s)
                {
                    CaretOffset = s;
                }
            });

            /*_analysisTriggerEvents.Select(_ => Observable.Timer(TimeSpan.FromMilliseconds(300)).ObserveOn(AvaloniaScheduler.Instance)
            .SelectMany(o => DoCodeAnalysisAsync())).Switch().Subscribe(_ => { });*/

            _analysisTriggerEvents.Throttle(TimeSpan.FromMilliseconds(300)).ObserveOn(AvaloniaScheduler.Instance).Subscribe(async _ =>
            {
                await DoCodeAnalysisAsync();
            });

            this.GetObservableWithHistory(SourceFileProperty).Subscribe((file) =>
            {
                if (file.Item1 != file.Item2)
                {
                    if (System.IO.File.Exists(file.Item2.Location))
                    {
                        using (var fs = System.IO.File.OpenText(file.Item2.Location))
                        {
                            Document = new TextDocument(fs.ReadToEnd())
                            {
                                FileName = file.Item2.Location
                            };
                        }
                    }

                    _isLoaded = true;

                    RegisterLanguageService(file.Item2);

                    TextArea.TextView.Redraw();
                }
            });

            TextArea.TextEntering += (sender, e) =>
            {
                _textEntering = true;
            };

            Observable.FromEventPattern(TextArea.Caret, nameof(TextArea.Caret.PositionChanged)).Subscribe(e =>
            {
                if (TextArea.Caret.Line != _lastLine && LanguageService != null)
                {
                    var line = Document.GetLineByNumber(TextArea.Caret.Line);

                    if (line.Length == 0)
                    {
                        _suppressIsDirtyNotifications = true;
                        LanguageService.IndentationStrategy.IndentLine(Document, line);
                        _suppressIsDirtyNotifications = false;
                    }
                }

                _lastLine = TextArea.Caret.Line;
            });


            Observable.FromEventPattern(TextArea.Caret, nameof(TextArea.Caret.PositionChanged)).Throttle(TimeSpan.FromMilliseconds(100)).ObserveOn(AvaloniaScheduler.Instance).Subscribe(e =>
            {
                if (_intellisenseManager != null && !_textEntering)
                {
                    if (TextArea.Selection.IsEmpty)
                    {
                        var location = Document.GetLocation(CaretOffset);
                        _intellisenseManager.SetCursor(CaretOffset, location.Line, location.Column, UnsavedFiles.ToList());
                    }
                    else if (_currentSnippetContext != null)
                    {
                        var offset = Document.GetOffset(TextArea.Selection.StartPosition.Location);
                        _intellisenseManager.SetCursor(offset, TextArea.Selection.StartPosition.Line, TextArea.Selection.StartPosition.Column, UnsavedFiles.ToList());
                    }
                }

                if (CaretOffset > 0)
                {
                    var prevLocation = new TextViewPosition(Document.GetLocation(CaretOffset - 1));

                    var visualLocation = TextArea.TextView.GetVisualPosition(prevLocation, VisualYPosition.LineBottom);
                    var visualLocationTop = TextArea.TextView.GetVisualPosition(prevLocation, VisualYPosition.LineTop);

                    var position = visualLocation - TextArea.TextView.ScrollOffset;
                    position = position.Transform(TextArea.TextView.TransformToVisual(TextArea).Value);

                    _intellisenseControl.SetLocation(position);
                    _completionAssistantControl.SetLocation(position);

                    _selectedWordBackgroundRenderer.SelectedWord = GetWordAtOffset(CaretOffset);

                    Line = TextArea.Caret.Line;
                    Column = TextArea.Caret.Column;
                    EditorCaretOffset = TextArea.Caret.Offset;

                    TextArea.TextView.InvalidateLayer(KnownLayer.Background);
                }
            });

            TextArea.TextEntered += (sender, e) =>
            {
                _intellisenseManager?.OnTextInput(e, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column);
                _textEntering = false;
            };

            _intellisense = new IntellisenseViewModel();

            _completionAssistant = new CompletionAssistantViewModel(_intellisense);

            EventHandler<KeyEventArgs> tunneledKeyUpHandler = (send, ee) =>
            {
                if (CaretOffset > 0)
                {
                    _intellisenseManager?.OnKeyUp(ee, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column);
                }
            };

            EventHandler<KeyEventArgs> tunneledKeyDownHandler = (send, ee) =>
            {
                if (CaretOffset > 0)
                {
                    _intellisenseManager?.OnKeyDown(ee, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column);

                    if (ee.Key == Key.Tab && _currentSnippetContext == null && LanguageService != null)
                    {
                        var wordStart = Document.FindPrevWordStart(CaretOffset);

                        if (wordStart > 0)
                        {
                            string word = Document.GetText(wordStart, CaretOffset - wordStart);

                            var codeSnippet = _snippetManager.GetSnippet(LanguageService, SourceFile.Project?.Solution, SourceFile.Project, word);

                            if (codeSnippet != null)
                            {
                                var snippet = SnippetParser.Parse(LanguageService, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column, codeSnippet.Snippet);

                                _intellisenseManager.CloseIntellisense();

                                using (Document.RunUpdate())
                                {
                                    Document.Remove(wordStart, CaretOffset - wordStart);

                                    _intellisenseManager.IncludeSnippets = false;
                                    _currentSnippetContext = snippet.Insert(TextArea);
                                }

                                if (_currentSnippetContext.ActiveElements.Count() > 0)
                                {
                                    IDisposable disposable = null;

                                    disposable = Observable.FromEventPattern<SnippetEventArgs>(_currentSnippetContext, nameof(_currentSnippetContext.Deactivated)).Take(1).Subscribe(o =>
                                    {
                                        _currentSnippetContext = null;
                                        _intellisenseManager.IncludeSnippets = true;

                                        disposable.Dispose();
                                    });
                                }
                                else
                                {
                                    _currentSnippetContext = null;
                                    _intellisenseManager.IncludeSnippets = true;
                                }
                            }
                        }
                    }
                }
            };

            AddHandler(KeyDownEvent, tunneledKeyDownHandler, RoutingStrategies.Tunnel);
            AddHandler(KeyUpEvent, tunneledKeyUpHandler, RoutingStrategies.Tunnel);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (_isLoaded)
            {
                if (!_suppressIsDirtyNotifications)
                {
                    IsDirty = true;

                    TriggerCodeAnalysis();
                }
            }
        }

        public async Task<Symbol> GetSymbolAsync(int offset)
        {
            if (LanguageService != null)
            {
                return await LanguageService.GetSymbolAsync(SourceFile, UnsavedFiles, offset);
            }

            return null;
        }

        public async Task<object> UpdateToolTipAsync()
        {
            if (VisualRoot == null)
            {
                return null;
            }

            var mouseDevice = (VisualRoot as IInputRoot)?.MouseDevice;
            var position = GetPositionFromPoint(mouseDevice.GetPosition(this));

            if (position.HasValue)
            {
                var offset = Document.GetOffset(position.Value.Location);

                var matching = Diagnostics?.FindSegmentsContaining(offset).FirstOrDefault();

                if (matching != null)
                {
                    return new ErrorProbeViewModel(matching);
                }

                var tooltipRequestEventArgs = new TooltipDataRequestEventArgs();

                RequestTooltipContent?.Invoke(this, tooltipRequestEventArgs);

                if (tooltipRequestEventArgs.GetViewModelAsyncTask != null)
                {
                    return await tooltipRequestEventArgs.GetViewModelAsyncTask(offset);
                }
            }

            return null;
        }

        public bool IsLoaded => _isLoaded;

        public TextSegment GetSelectionSegment()
        {
            TextSegment result = null;

            if (SelectionStart < (SelectionStart + SelectionLength))
            {
                result = new TextSegment { StartOffset = SelectionStart, EndOffset = (SelectionStart + SelectionLength) };
            }
            else
            {
                result = new TextSegment { StartOffset = (SelectionStart + SelectionLength), EndOffset = SelectionStart };
            }

            return result;
        }

        private void ModifySelectedLines(Action<int, int> action)
        {
            var selection = TextArea.Selection;

            var startLine = Document.GetLineByOffset(CaretOffset).LineNumber;
            var endLine = startLine;

            if (selection.Length > 0)
            {
                startLine = selection.StartPosition.Line;
                endLine = selection.EndPosition.Line;
            }

            var selectionSegment = GetSelectionSegment();
            var caretSegment = new TextSegment() { StartOffset = CaretOffset, EndOffset = CaretOffset };

            var anchors = new TextSegmentCollection<TextSegment>(Document);

            anchors.Add(selectionSegment);
            anchors.Add(caretSegment);

            action(startLine, endLine);

            SetSelection(selectionSegment);

            CaretOffset = caretSegment.StartOffset;

            Focus();
        }

        public void CommentSelection()
        {
            if (LanguageService != null)
            {
                ModifySelectedLines((start, end) =>
                {
                    LanguageService.Comment(SourceFile, Document, start, end, CaretOffset);
                });
            }
        }

        public void UncommentSelection()
        {
            if (LanguageService != null)
            {
                ModifySelectedLines((start, end) =>
                {
                    LanguageService.UnComment(SourceFile, Document, start, end, CaretOffset);
                });
            }
        }

        public void FormatAll()
        {
            if (LanguageService != null)
            {
                if (Settings.GetSettings<EditorSettings>().AutoFormat)
                {
                var caretOffset = LanguageService.Format(SourceFile, Document, 0, (uint)Document.TextLength, CaretOffset);

                // some language services manually set the caret themselves and return -1 to indicate this.
                if(caretOffset >= 0)
                {
                    CaretOffset = caretOffset;
                }

                Focus();
                }
            }
        }

        public void SetSelection(TextSegment segment)
        {
            SelectionStart = segment.StartOffset;
            SelectionLength = segment.Length;
        }

        private void StartBackgroundWorkers()
        {
            _cancellationSource = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                _codeAnalysisRunner.RunLoop(_cancellationSource.Token);
                _cancellationSource = null;
            });
        }

        public void ShutdownBackgroundWorkers()
        {
            _cancellationSource?.Cancel();
        }

        public void Save()
        {
            if (SourceFile != null && Document != null && IsDirty)
            {
                if (Settings.GetSettings<EditorSettings>().RemoveTrailingWhitespaceOnSave)
                {
                    Document.TrimTrailingWhiteSpace();
                }

                System.IO.File.WriteAllText(SourceFile.Location, Document.Text);
                IsDirty = false;

                lock (UnsavedFiles)
                {
                    var unsavedFile = UnsavedFiles.BinarySearch(SourceFile.Location);

                    if (unsavedFile != null)
                    {
                        UnsavedFiles.Remove(unsavedFile);
                    }
                }
            }
        }

        private async Task<bool> DoCodeAnalysisAsync()
        {
            var sourceFile = SourceFile;
            var unsavedFiles = UnsavedFiles.ToList();
            var document = Document;

            await _codeAnalysisRunner.InvokeAsync(async () =>
            {
                if (LanguageService != null)
                {
                    var result = await LanguageService.RunCodeAnalysisAsync(sourceFile, document, unsavedFiles, () => false);

                    _textColorizer?.SetTransformations(result.SyntaxHighlightingData);

                    _diagnosticMarkersRenderer?.SetDiagnostics(result.Diagnostics);

                    _scopeLineBackgroundRenderer?.ApplyIndex(result.IndexItems);

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        TextArea.TextView.Redraw();
                    });
                }
            });

            return true;
        }

        private void RegisterLanguageService(ISourceFile sourceFile)
        {
            UnRegisterLanguageService();

            if (sourceFile.Project?.Solution != null)
            {
                _snippetManager.InitialiseSnippetsForSolution(sourceFile.Project.Solution);
            }

            if (sourceFile.Project != null)
            {
                _snippetManager.InitialiseSnippetsForProject(sourceFile.Project);
            }

            LanguageService = _shell.LanguageServices.FirstOrDefault(o => o.CanHandle(sourceFile));

            if (LanguageService != null)
            {
                LanguageServiceName = LanguageService.Title;

                LanguageService.RegisterSourceFile(this, sourceFile, Document);

                _diagnosticMarkersRenderer = new TextMarkerService(Document);
                _textColorizer = new TextColoringTransformer(Document);
                _scopeLineBackgroundRenderer = new ScopeLineBackgroundRenderer(Document);

                TextArea.TextView.BackgroundRenderers.Add(_scopeLineBackgroundRenderer);
                TextArea.TextView.BackgroundRenderers.Add(_diagnosticMarkersRenderer);
                TextArea.TextView.LineTransformers.Insert(0, _textColorizer);

                _intellisenseManager = new IntellisenseManager(this, _intellisense, _completionAssistant, LanguageService, sourceFile);

                TextArea.IndentationStrategy = LanguageService.IndentationStrategy;

                LanguageService.Diagnostics.ObserveOn(AvaloniaScheduler.Instance).Subscribe(d =>
                {
                    Diagnostics = d;

                    _shell.InvalidateErrors();
                });
            }
            else
            {
                LanguageService = null;
                LanguageServiceName = "Plain Text";
            }

            StartBackgroundWorkers();

            Document.TextChanged += TextDocument_TextChanged;

            DoCodeAnalysisAsync().GetAwaiter();
        }

        public void UnRegisterLanguageService()
        {
            if (_scopeLineBackgroundRenderer != null)
            {
                TextArea.TextView.BackgroundRenderers.Remove(_scopeLineBackgroundRenderer);
            }

            if (_textColorizer != null)
            {
                TextArea.TextView.LineTransformers.Remove(_textColorizer);
                _textColorizer = null;
            }

            if (_diagnosticMarkersRenderer != null)
            {
                TextArea.TextView.BackgroundRenderers.Remove(_diagnosticMarkersRenderer);
                _diagnosticMarkersRenderer = null;
            }

            ShutdownBackgroundWorkers();

            UnsavedFile unsavedFile = null;

            lock (UnsavedFiles)
            {
                unsavedFile = UnsavedFiles.BinarySearch(SourceFile.Location);
            }

            if (unsavedFile != null)
            {
                lock (UnsavedFiles)
                {
                    UnsavedFiles.Remove(unsavedFile);
                }
            }

            if (LanguageService != null)
            {
                LanguageService.UnregisterSourceFile(this, SourceFile);
            }

            Document.TextChanged -= TextDocument_TextChanged;
        }

        private void TextDocument_TextChanged(object sender, EventArgs e)
        {
            UnsavedFile unsavedFile = null;

            lock (UnsavedFiles)
            {
                unsavedFile = UnsavedFiles.BinarySearch(SourceFile.Location);
            }

            if (unsavedFile == null)
            {
                lock (UnsavedFiles)
                {
                    UnsavedFiles.InsertSorted(new UnsavedFile(SourceFile.Location, Document.Text));
                }
            }
            else
            {
                unsavedFile.Contents = Document.Text;
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _toolTip = e.NameScope.Find<CodeEditorToolTip>("PART_Tooltip");
            _toolTip.AttachEditor(this);

            _intellisenseControl = e.NameScope.Find<Intellisense>("PART_Intellisense");
            _completionAssistantControl = e.NameScope.Find<CompletionAssistantView>("PART_CompletionAssistant");

            _intellisenseControl.PlacementTarget = TextArea;
            _intellisenseControl.DataContext = _intellisense;

            _completionAssistantControl.PlacementTarget = TextArea;
            _completionAssistantControl.DataContext = _completionAssistant;
        }

        private void List_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var transformer in e.NewItems)
                    {
                        TextArea.TextView.LineTransformers.Add(transformer as IVisualLineTransformer);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    foreach (var transformer in e.OldItems)
                    {
                        TextArea.TextView.LineTransformers.Remove(transformer as IVisualLineTransformer);
                    }
                    break;
            }
        }

        public static readonly StyledProperty<int> CaretOffsetProperty =
            AvaloniaProperty.Register<CodeEditor, int>(nameof(EditorCaretOffset), defaultBindingMode: BindingMode.TwoWay);

        public int EditorCaretOffset
        {
            get { return GetValue(CaretOffsetProperty); }
            set { SetValue(CaretOffsetProperty, value); }
        }

        public static readonly StyledProperty<bool> ShowBreakpointsProperty =
            AvaloniaProperty.Register<CodeEditor, bool>(nameof(ShowBreakpoints), true);

        public bool ShowBreakpoints
        {
            get { return GetValue(ShowBreakpointsProperty); }
            set { SetValue(ShowBreakpointsProperty, value); }
        }

        public static readonly StyledProperty<bool> LineNumbersVisibleProperty =
            AvaloniaProperty.Register<CodeEditor, bool>(nameof(LineNumbersVisible), true);

        public bool LineNumbersVisible
        {
            get { return GetValue(LineNumbersVisibleProperty); }
            set { SetValue(LineNumbersVisibleProperty, value); }
        }

        public static readonly StyledProperty<bool> HighlightSelectedLineProperty =
            AvaloniaProperty.Register<CodeEditor, bool>(nameof(HighlightSelectedLine), true);

        public bool HighlightSelectedLine
        {
            get { return GetValue(HighlightSelectedLineProperty); }
            set { SetValue(HighlightSelectedLineProperty, value); }
        }

        public static readonly StyledProperty<bool> HighlightSelectedWordProperty =
            AvaloniaProperty.Register<CodeEditor, bool>(nameof(HighlightSelectedWord), true);

        public bool HighlightSelectedWord
        {
            get { return GetValue(HighlightSelectedWordProperty); }
            set { SetValue(HighlightSelectedWordProperty, value); }
        }

        public static readonly StyledProperty<bool> ShowColumnLimitProperty =
            AvaloniaProperty.Register<CodeEditor, bool>(nameof(ShowColumnLimit), false);

        public bool ShowColumnLimit
        {
            get { return GetValue(ShowColumnLimitProperty); }
            set { SetValue(ShowColumnLimitProperty, value); }
        }

        public static readonly StyledProperty<UInt32> ColumnLimitProperty =
            AvaloniaProperty.Register<CodeEditor, UInt32>(nameof(ColumnLimit), 80);

        public UInt32 ColumnLimit
        {
            get { return GetValue(ColumnLimitProperty); }
            set { SetValue(ColumnLimitProperty, value); }
        }

        public static readonly StyledProperty<int> LineProperty =
            AvaloniaProperty.Register<CodeEditor, int>(nameof(Line), defaultBindingMode: BindingMode.TwoWay);

        public int Line
        {
            get { return GetValue(LineProperty); }
            set { SetValue(LineProperty, value); }
        }

        public static readonly StyledProperty<int> ColumnProperty =
            AvaloniaProperty.Register<CodeEditor, int>(nameof(Column), defaultBindingMode: BindingMode.TwoWay);

        public int Column
        {
            get { return GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }

        public static readonly StyledProperty<string> LanguageServiceNameProperty =
            AvaloniaProperty.Register<CodeEditor, string>(nameof(LanguageServiceName), defaultBindingMode: BindingMode.TwoWay);

        public string LanguageServiceName
        {
            get { return GetValue(LanguageServiceNameProperty); }
            set { SetValue(LanguageServiceNameProperty, value); }
        }


        public static readonly StyledProperty<ObservableCollection<IVisualLineTransformer>> DocumentLineTransformersProperty =
            AvaloniaProperty.Register<CodeEditor, ObservableCollection<IVisualLineTransformer>>(nameof(DocumentLineTransformers), new ObservableCollection<IVisualLineTransformer>());

        public ObservableCollection<IVisualLineTransformer> DocumentLineTransformers
        {
            get { return GetValue(DocumentLineTransformersProperty); }
            set { SetValue(DocumentLineTransformersProperty, value); }
        }

        public static readonly StyledProperty<ObservableCollection<IBackgroundRenderer>> BackgroundRenderersProperty =
            AvaloniaProperty.Register<CodeEditor, ObservableCollection<IBackgroundRenderer>>(nameof(BackgroundRenderers), new ObservableCollection<IBackgroundRenderer>(), defaultBindingMode: BindingMode.TwoWay);

        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return GetValue(BackgroundRenderersProperty); }
            set { SetValue(BackgroundRenderersProperty, value); }
        }

        public static readonly StyledProperty<ColorScheme> ColorSchemeProperty =
            AvaloniaProperty.Register<CodeEditor, ColorScheme>(nameof(ColorScheme));

        public ColorScheme ColorScheme
        {
            get => GetValue(ColorSchemeProperty);
            set => SetValue(ColorSchemeProperty, value);
        }

        public static readonly StyledProperty<TextSegmentCollection<Diagnostic>> DiagnosticsProperty =
            AvaloniaProperty.Register<CodeEditor, TextSegmentCollection<Diagnostic>>(nameof(Diagnostics), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        public TextSegmentCollection<Diagnostic> Diagnostics
        {
            get { return GetValue(DiagnosticsProperty); }
            set { SetValue(DiagnosticsProperty, value); }
        }


        public static readonly AvaloniaProperty<ISourceFile> SourceFileProperty =
            AvaloniaProperty.Register<CodeEditor, ISourceFile>(nameof(SourceFile));

        public ISourceFile SourceFile
        {
            get { return GetValue(SourceFileProperty); }
            set { SetValue(SourceFileProperty, value); }
        }

        public static readonly StyledProperty<bool> IsDirtyProperty =
            AvaloniaProperty.Register<CodeEditor, bool>(nameof(IsDirty), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        public bool IsDirty
        {
            get { return GetValue(IsDirtyProperty); }
            set { SetValue(IsDirtyProperty, value); }
        }

        public int GetOffsetFromPoint(Point point)
        {
            var position = GetPositionFromPoint(point);

            var offset = position != null ? Document.GetOffset(position.Value.Location) : -1;

            return offset;
        }

        public string GetWordAtOffset(int offset)
        {
            var result = string.Empty;

            if (offset >= 0 && Document.TextLength > offset)
            {
                var start = offset;

                var currentChar = Document.GetCharAt(offset);
                var prevChar = '\0';

                if (offset > 0)
                {
                    prevChar = Document.GetCharAt(offset - 1);
                }

                var charClass = TextUtilities.GetCharacterClass(currentChar);

                if (charClass != CharacterClass.LineTerminator && prevChar != ' ' &&
                    TextUtilities.GetCharacterClass(prevChar) != CharacterClass.LineTerminator)
                {
                    start = TextUtilities.GetNextCaretPosition(Document, offset, LogicalDirection.Backward,
                        CaretPositioningMode.WordStart);
                }

                var end = TextUtilities.GetNextCaretPosition(Document, start, LogicalDirection.Forward,
                    CaretPositioningMode.WordBorder);

                if (start != -1 && end != -1)
                {
                    var word = Document.GetText(start, end - start).Trim();

                    if (word.IsSymbol())
                    {
                        result = word;
                    }
                }
            }

            return result;
        }

        public void SetDebugHighlight(int line, int startColumn, int endColumn)
        {
            if (startColumn == -1 && endColumn == -1)
            {
                var docLine = Document.GetLineByNumber(line);

                var leading = TextUtilities.GetLeadingWhitespace(Document, docLine);

                var trailing = TextUtilities.GetTrailingWhitespace(Document, docLine);

                startColumn = Document.GetLocation(leading.EndOffset).Column;
                endColumn = Document.GetLocation(trailing.Offset).Column;
            }

            _selectedDebugLineBackgroundRenderer.SetLocation(line, startColumn, endColumn);
        }

        public void ClearDebugHighlight()
        {
            _selectedDebugLineBackgroundRenderer.SetLocation(-1);
        }
    }
}
