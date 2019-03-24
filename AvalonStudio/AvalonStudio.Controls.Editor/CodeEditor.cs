using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Rendering;
using AvaloniaEdit.Snippets;
using AvalonStudio.Controls.Editor.ContextActions;
using AvalonStudio.Controls.Editor.Highlighting;
using AvalonStudio.Controls.Editor.Snippets;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Languages;
using AvalonStudio.Shell;
using AvalonStudio.TextEditor.Rendering;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Editor
{
    public class CodeEditor : AvaloniaEdit.TextEditor
    {
        public static List<UnsavedFile> UnsavedFiles
        {
            get
            {
                return IoC.Get<IShell>().Documents.OfType<ITextDocumentTabViewModel>()
                .Where(x => x.IsDirty)
                .Select(x => new UnsavedFile(x.SourceFile.FilePath, x.Document.Text))
                .ToList();
            }
        }

        private SnippetManager _snippetManager;
        private InsertionContext _currentSnippetContext;

        public IntellisenseViewModel Intellisense { get; }
        private Intellisense _intellisenseControl;
        private IntellisenseManager _intellisenseManager;

        private RenameControl _renameControl;

        private CompletionAssistantView _completionAssistantControl;
        private CompletionAssistantViewModel _completionAssistant;

        private SelectedDebugLineBackgroundRenderer _selectedDebugLineBackgroundRenderer;

        private SelectedWordBackgroundRenderer _selectedWordBackgroundRenderer;

        private ColumnLimitBackgroundRenderer _columnLimitBackgroundRenderer;

        private TextMarkerService _diagnosticMarkersRenderer;

        private TextColoringTransformer _textColorizer;

        private ScopeLineBackgroundRenderer _scopeLineBackgroundRenderer;
        private BracketMatchingBackgroundRenderer _bracketMatchingBackgroundRenderer;

        private bool _isLoaded = false;

        private int _lastLine = -1;

        private bool _textEntering;
        private readonly IShell _shell;
        private CodeEditorToolTip _toolTip;
        private LineNumberMargin _lineNumberMargin;
        private BreakPointMargin _breakpointMargin;
        private SelectedLineBackgroundRenderer _selectedLineBackgroundRenderer;
        private CompositeDisposable _disposables;

        private ContextActionsRenderer _contextActionsRenderer;

        public CodeEditor() : base(new TextArea(), null)
        {
            TextArea.IndentationStrategy = null;

            _shell = IoC.Get<IShell>();

            _snippetManager = IoC.Get<SnippetManager>();

            _lineNumberMargin = new LineNumberMargin(this);

            _breakpointMargin = new BreakPointMargin(this, IoC.Get<IDebugManager2>()?.Breakpoints);

            _selectedLineBackgroundRenderer = new SelectedLineBackgroundRenderer(this);
            _bracketMatchingBackgroundRenderer = new BracketMatchingBackgroundRenderer(this);

            _selectedWordBackgroundRenderer = new SelectedWordBackgroundRenderer();

            _columnLimitBackgroundRenderer = new ColumnLimitBackgroundRenderer();

            _selectedDebugLineBackgroundRenderer = new SelectedDebugLineBackgroundRenderer();

            TextArea.TextView.Margin = new Thickness(10, 0, 0, 0);

            TextArea.TextView.BackgroundRenderers.Add(_selectedDebugLineBackgroundRenderer);
            TextArea.TextView.LineTransformers.Add(_selectedDebugLineBackgroundRenderer);
            TextArea.TextView.BackgroundRenderers.Add(_bracketMatchingBackgroundRenderer);

            TextArea.SelectionBrush = Brush.Parse("#AA569CD6");
            TextArea.SelectionCornerRadius = 0;

            void tunneledKeyUpHandler(object send, KeyEventArgs ee)
            {
                if (CaretOffset > 0)
                {
                    _intellisenseManager?.OnKeyUp(ee, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column);
                }
            }

            void tunneledKeyDownHandler(object send, KeyEventArgs ee)
            {
                if (CaretOffset > 0)
                {
                    _intellisenseManager?.OnKeyDown(ee, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column);

                    if (ee.Key == Key.Tab && _currentSnippetContext == null && Editor is ICodeEditor codeEditor && codeEditor.LanguageService != null)
                    {
                        var wordStart = Document.FindPrevWordStart(CaretOffset);

                        if (wordStart > 0)
                        {
                            string word = Document.GetText(wordStart, CaretOffset - wordStart);

                            var codeSnippet = _snippetManager.GetSnippet(codeEditor.LanguageService, Editor.SourceFile.Project?.Solution, Editor.SourceFile.Project, word);

                            if (codeSnippet != null)
                            {
                                var snippet = SnippetParser.Parse(codeEditor.LanguageService, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column, codeSnippet.Snippet);

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
            }

            _disposables = new CompositeDisposable {
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
            }),

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
            }),

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
            }),

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
            }),

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
            }),

            this.GetObservable(ColumnLimitProperty).Subscribe(limit =>
            {
                _columnLimitBackgroundRenderer.Column = limit;
                this.TextArea.TextView.InvalidateLayer(KnownLayer.Background);
            }),

            this.GetObservable(ContextActionsIconProperty).Subscribe(icon =>
            {
                if(_contextActionsRenderer != null)
                {
                    _contextActionsRenderer.IconImage = icon;
                }
            }),

            this.GetObservable(ColorSchemeProperty).Subscribe(colorScheme =>
            {
                if (colorScheme != null)
                {
                    Background = colorScheme.Background;
                    Foreground = colorScheme.Text;

                    _lineNumberMargin.Background = colorScheme.Background;

                    if(_diagnosticMarkersRenderer != null)
                    {
                        _diagnosticMarkersRenderer.ColorScheme = colorScheme;
                    }

                    _textColorizer?.RecalculateBrushes();
                    TextArea.TextView.InvalidateLayer(KnownLayer.Background);
                    TextArea.TextView.Redraw();
                }
            }),

            this.GetObservable(EditorCaretOffsetProperty).Subscribe(s =>
            {
                if (Document?.TextLength >= s)
                {
                    CaretOffset = s;
                    TextArea.Caret.BringCaretToView();
                }
            }),

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
            }),

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
            }),

            Observable.FromEventPattern(TextArea.Caret, nameof(TextArea.Caret.PositionChanged)).Subscribe(e =>
            {
                if(_isLoaded && Document != null)
                {
                    _lastLine = TextArea.Caret.Line;

                    Line = TextArea.Caret.Line;
                    Column = TextArea.Caret.Column;
                    EditorCaretOffset = TextArea.Caret.Offset;

                    var location = new TextViewPosition(Document.GetLocation(CaretOffset));

                    var visualLocation = TextArea.TextView.GetVisualPosition(location, VisualYPosition.LineBottom);
                    var visualLocationTop = TextArea.TextView.GetVisualPosition(location, VisualYPosition.LineTop);

                    var position = visualLocation - TextArea.TextView.ScrollOffset;
                    position = position.Transform(TextArea.TextView.TransformToVisual(TextArea).Value);

                    _intellisenseControl.SetLocation(position);
                }
            }),

            Observable.FromEventPattern(TextArea.Caret, nameof(TextArea.Caret.PositionChanged)).Throttle(TimeSpan.FromMilliseconds(100)).ObserveOn(AvaloniaScheduler.Instance).Subscribe(e =>
            {
                if(Document != null)
                {
                    var location = new TextViewPosition(Document.GetLocation(CaretOffset));

                    if (_intellisenseManager != null && !_textEntering)
                    {
                        if (TextArea.Selection.IsEmpty)
                        {
                            _intellisenseManager.SetCursor(CaretOffset, location.Line, location.Column, UnsavedFiles.ToList());
                        }
                        else if (_currentSnippetContext != null)
                        {
                            var offset = Document.GetOffset(TextArea.Selection.StartPosition.Location);
                            _intellisenseManager.SetCursor(offset, TextArea.Selection.StartPosition.Line, TextArea.Selection.StartPosition.Column, UnsavedFiles.ToList());
                        }
                    }

                    _selectedWordBackgroundRenderer.SelectedWord = GetWordAtOffset(CaretOffset);

                    TextArea.TextView.InvalidateLayer(KnownLayer.Background);
                }
            }),

            this.WhenAnyValue(x=>x.DebugHighlight).Where(loc => loc != null).Subscribe(location =>
            {
                if(location.Line != -1)
                {
                    SetDebugHighlight(location.Line, location.StartColumn, location.EndColumn);
                }
                else
                {
                    ClearDebugHighlight();
                }
            }),
            this.GetObservable(EditorProperty).Subscribe(editor =>
            {
                if(editor != null)
                {
                    if (editor.SourceFile.Project?.Solution != null)
                    {
                        _snippetManager.InitialiseSnippetsForSolution(editor.SourceFile.Project.Solution);
                    }

                    if (editor.SourceFile.Project != null)
                    {
                        _snippetManager.InitialiseSnippetsForProject(editor.SourceFile.Project);
                    }

                    SyntaxHighlighting = CustomHighlightingManager.Instance.GetDefinition(editor.SourceFile.ContentType);

                    if(editor.Document is AvalonStudioTextDocument td && Document != td.Document)
                    {
                        Document = td.Document;

                        if(editor.Offset <= Document.TextLength)
                        {
                            CaretOffset = editor.Offset;
                        }

                        _textColorizer = new TextColoringTransformer(Document);
                        _scopeLineBackgroundRenderer = new ScopeLineBackgroundRenderer(Document);


                        TextArea.TextView.BackgroundRenderers.Add(_scopeLineBackgroundRenderer);
                        TextArea.TextView.LineTransformers.Insert(0, _textColorizer);

                        _diagnosticMarkersRenderer = new TextMarkerService(Document);
                        _contextActionsRenderer = new ContextActionsRenderer(this, _diagnosticMarkersRenderer);
                        TextArea.LeftMargins.Add(_contextActionsRenderer);
                        TextArea.TextView.BackgroundRenderers.Add(_diagnosticMarkersRenderer);
                    }

                    if(editor is ICodeEditor codeEditor)
                    {
                        if(codeEditor.Highlights != null)
                        {
                            _disposables.Add(
                            Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(codeEditor.Highlights, nameof(codeEditor.Highlights.CollectionChanged))
                            .Subscribe(observer =>
                            {
                                var e = observer.EventArgs;

                                switch(e.Action)
                                {
                                    case NotifyCollectionChangedAction.Add:
                                        foreach(var (tag,highlightList)in  e.NewItems.Cast<(object tag, SyntaxHighlightDataList highlightList)>())
                                        {
                                            _textColorizer.SetTransformations(tag, highlightList);
                                        }
                                        break;

                                    case NotifyCollectionChangedAction.Remove:
                                        foreach(var (tag,highlightList)in  e.OldItems.Cast<(object tag, SyntaxHighlightDataList highlightList)>())
                                        {
                                            _textColorizer.RemoveAll(i => i.Tag == tag);
                                        }
                                        break;

                                    case NotifyCollectionChangedAction.Reset:
                                        foreach(var (tag,highlightList)in  e.OldItems.Cast<(object tag, SyntaxHighlightDataList highlightList)>())
                                        {
                                            _textColorizer.RemoveAll(i => true);
                                        }
                                        break;

                                       default:
                                        throw new NotSupportedException();
                                }

                                TextArea.TextView.Redraw();
                            }));

                            _disposables.Add(
                            Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(codeEditor.Diagnostics, nameof(codeEditor.Diagnostics.CollectionChanged))
                            .Subscribe(observer =>
                            {
                                var e = observer.EventArgs;

                                switch(e.Action)
                                {
                                    case NotifyCollectionChangedAction.Add:
                                        foreach(var (tag,diagnostics)in  e.NewItems.Cast<(object tag, IEnumerable<Diagnostic> diagnostics)>())
                                        {
                                            _diagnosticMarkersRenderer.SetDiagnostics(tag, diagnostics);
                                        }
                                        break;

                                    case NotifyCollectionChangedAction.Remove:
                                        foreach(var (tag,diagnostics)in  e.OldItems.Cast<(object tag, IEnumerable<Diagnostic> diagnostics)>())
                                        {
                                            _diagnosticMarkersRenderer.RemoveAll(x=>x.Tag == tag);
                                        }
                                        break;

                                    case NotifyCollectionChangedAction.Reset:
                                        foreach(var (tag,diagnostics)in  e.OldItems.Cast<(object tag, IEnumerable<Diagnostic> diagnostics)>())
                                        {
                                            _diagnosticMarkersRenderer.RemoveAll(i => true);
                                        }
                                        break;

                                       default:
                                        throw new NotSupportedException();
                                }

                                TextArea.TextView.Redraw();
                                _contextActionsRenderer.OnDiagnosticsUpdated();
                            }));

                            _disposables.Add(codeEditor.WhenAnyValue(x =>x.CodeIndex).Subscribe(codeIndex =>
                            {
                                _scopeLineBackgroundRenderer.ApplyIndex(codeIndex);
                            }));

                            _scopeLineBackgroundRenderer.ApplyIndex(codeEditor.CodeIndex);

                            foreach(var (tag,diagnostics)in codeEditor.Diagnostics)
                            {
                                _diagnosticMarkersRenderer.SetDiagnostics(tag, diagnostics);
                            }

                            foreach(var (tag,highlights)in codeEditor.Highlights)
                            {
                                _textColorizer.SetTransformations(tag, highlights);
                            }

                            TextArea.TextView.Redraw();
                        }

                        _intellisenseManager = new IntellisenseManager(editor, Intellisense, _completionAssistant, codeEditor.LanguageService, editor.SourceFile, offset =>
                        {
                            var location = new TextViewPosition(Document.GetLocation(offset));

                            var visualLocation = TextArea.TextView.GetVisualPosition(location, VisualYPosition.LineBottom);
                            var visualLocationTop = TextArea.TextView.GetVisualPosition(location, VisualYPosition.LineTop);

                            var position = visualLocation - TextArea.TextView.ScrollOffset;
                            position = position.Transform(TextArea.TextView.TransformToVisual(TextArea).Value);

                            _completionAssistantControl.SetLocation(position);
                        });

                        _disposables.Add(_intellisenseManager);

                        foreach (var contextActionProvider in codeEditor.LanguageService.GetContextActionProviders())
                        {
                            _contextActionsRenderer.Providers.Add(contextActionProvider);
                        }
                    }

                    Dispatcher.UIThread.Post(()=>
                    {
                        TextArea.ScrollToLine(Line);
                        Focus();
                    });
                }
                else
                {
                    if(Document != null)
                    {
                        Document = null;
                    }
                }
            }),
            this.GetObservable(RenameOpenProperty).Subscribe(open =>
            {
                if(_isLoaded && Editor != null)
                {
                    var token = Editor.Document.GetToken(CaretOffset);
                    var location = new TextViewPosition(Document.GetLocation(token.Offset));

                    var visualLocation = TextArea.TextView.GetVisualPosition(location, VisualYPosition.LineBottom);
                    var visualLocationTop = TextArea.TextView.GetVisualPosition(location, VisualYPosition.LineTop);

                    var position = visualLocation - TextArea.TextView.ScrollOffset;
                    position = position.Transform(TextArea.TextView.TransformToVisual(TextArea).Value);

                    _renameControl.SetLocation(position);
                    _renameControl.Open(this, Editor.Document.GetText(token));
                }
            }),

            AddHandler(KeyDownEvent, tunneledKeyDownHandler, RoutingStrategies.Tunnel),
            AddHandler(KeyUpEvent, tunneledKeyUpHandler, RoutingStrategies.Tunnel)
        };

            Options = new AvaloniaEdit.TextEditorOptions
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4,
                EnableHyperlinks = false,
                EnableEmailHyperlinks = false,
            };

            //BackgroundRenderersProperty.Changed.Subscribe(s =>
            //{
            //    if (s.Sender == this)
            //    {
            //        if (s.OldValue != null)
            //        {
            //            foreach (var renderer in (ObservableCollection<IBackgroundRenderer>)s.OldValue)
            //            {
            //                TextArea.TextView.BackgroundRenderers.Remove(renderer);
            //            }
            //        }

            //        if (s.NewValue != null)
            //        {
            //            foreach (var renderer in (ObservableCollection<IBackgroundRenderer>)s.NewValue)
            //            {
            //                TextArea.TextView.BackgroundRenderers.Add(renderer);
            //            }
            //        }
            //    }
            //});

            //DocumentLineTransformersProperty.Changed.Subscribe(s =>
            //{
            //    if (s.Sender == this)
            //    {
            //        if (s.OldValue != null)
            //        {
            //            foreach (var renderer in (ObservableCollection<IVisualLineTransformer>)s.OldValue)
            //            {
            //                TextArea.TextView.LineTransformers.Remove(renderer);
            //            }
            //        }

            //        if (s.NewValue != null)
            //        {
            //            foreach (var renderer in (ObservableCollection<IVisualLineTransformer>)s.NewValue)
            //            {
            //                TextArea.TextView.LineTransformers.Add(renderer);
            //            }
            //        }
            //    }
            //});


            /*_analysisTriggerEvents.Select(_ => Observable.Timer(TimeSpan.FromMilliseconds(300)).ObserveOn(AvaloniaScheduler.Instance)
            .SelectMany(o => DoCodeAnalysisAsync())).Switch().Subscribe(_ => { });*/

            Intellisense = new IntellisenseViewModel();

            _completionAssistant = new CompletionAssistantViewModel(Intellisense);

            TextArea.TextEntering += TextArea_TextEntering;

            TextArea.TextEntered += TextArea_TextEntered;
        }

        ~CodeEditor()
        {
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (_isLoaded)
            {
                Editor?.OnTextChanged();
            }
        }

        public async Task<object> UpdateToolTipAsync()
        {
            if (VisualRoot == null)
            {
                return null;
            }

            var mouseDevice = (VisualRoot as IInputRoot)?.MouseDevice;
            var position = GetPositionFromPoint(mouseDevice.GetPosition(this));

            if (position.HasValue && Editor != null)
            {
                var offset = Document.GetOffset(position.Value.Location);

                var matching = _diagnosticMarkersRenderer?.GetMarkersAtOffset(offset).FirstOrDefault()?.Diagnostic;

                if (matching != null && matching.Level != DiagnosticLevel.Hidden)
                {
                    return new ErrorProbeViewModel(matching);
                }

                return await Editor.GetToolTipContentAsync(offset);
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

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            _intellisenseManager?.CloseIntellisense();

            _completionAssistant?.Close();
        }

        public void SetSelection(TextSegment segment)
        {
            SelectionStart = segment.StartOffset;
            SelectionLength = segment.Length;
        }

        private void TextArea_TextEntered(object sender, TextInputEventArgs e)
        {
            if (Editor != null)
            {
                e.Handled = Editor.OnTextEntered(e.Text);
            }

            _intellisenseManager?.OnTextInput(e, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column);
            _textEntering = false;
        }

        private void TextArea_TextEntering(object sender, TextInputEventArgs e)
        {
            if (Editor != null)
            {
                e.Handled = Editor.OnBeforeTextEntered(e.Text);
            }

            _textEntering = true;
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _toolTip = e.NameScope.Find<CodeEditorToolTip>("PART_Tooltip");
            _toolTip.AttachEditor(this);

            _renameControl = e.NameScope.Find<RenameControl>("PART_RenameControl");
            _renameControl.PlacementTarget = TextArea;

            _intellisenseControl = e.NameScope.Find<Intellisense>("PART_Intellisense");
            _completionAssistantControl = e.NameScope.Find<CompletionAssistantView>("PART_CompletionAssistant");

            _intellisenseControl.SetSignatureHelper(_completionAssistantControl);

            _intellisenseControl.PlacementTarget = TextArea;
            _intellisenseControl.DataContext = Intellisense;

            _completionAssistantControl.PlacementTarget = TextArea;
            _completionAssistantControl.DataContext = _completionAssistant;

            _isLoaded = true;

            TextArea.SelectionChanged += TextArea_SelectionChanged;

            Dispatcher.UIThread.Post(() =>
            {
                Focus();
                TextArea.Caret.BringCaretToView();
            });
        }

        private void TextArea_SelectionChanged(object sender, EventArgs e)
        {
            if (TextArea.Selection.IsEmpty)
            {
                Selection = null;
            }
            else
            {
                var start = TextArea.Selection.StartPosition.Location;
                var end = TextArea.Selection.EndPosition.Location;

                if (end >= start)
                {
                    Selection = new Documents.SimpleSegment(Document.GetOffset(start), Document.GetOffset(end) - Document.GetOffset(start));
                }
                else
                {
                    Selection = new Documents.SimpleSegment(Document.GetOffset(end), Document.GetOffset(start) - Document.GetOffset(end));
                }
            }
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

        public static readonly StyledProperty<IBitmap> ContextActionsIconProperty = AvaloniaProperty.Register<CodeEditor, IBitmap>(nameof(ContextActionsIcon));

        public IBitmap ContextActionsIcon
        {
            get => this.GetValue(ContextActionsIconProperty);
            set => this.SetValue(ContextActionsIconProperty, value);
        }

        public static readonly StyledProperty<int> EditorCaretOffsetProperty =
            AvaloniaProperty.Register<CodeEditor, int>(nameof(EditorCaretOffset), defaultBindingMode: BindingMode.TwoWay);

        public int EditorCaretOffset
        {
            get { return GetValue(EditorCaretOffsetProperty); }
            set { SetValue(EditorCaretOffsetProperty, value); }
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

        public static readonly StyledProperty<bool> IsDirtyProperty =
            AvaloniaProperty.Register<CodeEditor, bool>(nameof(IsDirty), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        public bool IsDirty
        {
            get { return GetValue(IsDirtyProperty); }
            set { SetValue(IsDirtyProperty, value); }
        }

        public static readonly StyledProperty<DebugHighlightLocation> DebugHighlightProperty =
            AvaloniaProperty.Register<CodeEditor, DebugHighlightLocation>(nameof(DebugHighlight));

        public DebugHighlightLocation DebugHighlight
        {
            get => GetValue(DebugHighlightProperty);
            set => SetValue(DebugHighlightProperty, value);
        }

        public static readonly AvaloniaProperty<ITextEditor> EditorProperty =
            AvaloniaProperty.Register<CodeEditor, ITextEditor>(nameof(Editor));

        public ITextEditor Editor
        {
            get => GetValue(EditorProperty);
            set => SetValue(EditorProperty, value);
        }

        public static readonly AvaloniaProperty<Documents.ISegment> SelectionProperty =
            AvaloniaProperty.Register<CodeEditor, Documents.ISegment>(nameof(Selection), defaultBindingMode: BindingMode.TwoWay);

        public Documents.ISegment Selection
        {
            get => GetValue(SelectionProperty);
            set => SetValue(SelectionProperty, value);
        }

        public static readonly AvaloniaProperty<string> RenameTextProperty =
            AvaloniaProperty.Register<CodeEditor, string>(nameof(RenameText), defaultBindingMode: BindingMode.TwoWay);

        public string RenameText
        {
            get => GetValue(RenameTextProperty);
            set => SetValue(RenameTextProperty, value);
        }

        public static readonly AvaloniaProperty<bool> RenameOpenProperty =
            AvaloniaProperty.Register<CodeEditor, bool>(nameof(RenameOpen), defaultBindingMode: BindingMode.TwoWay);

        public bool RenameOpen
        {
            get => GetValue(RenameOpenProperty);
            set => SetValue(RenameOpenProperty, value);
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

                var charClass = AvaloniaEdit.Document.TextUtilities.GetCharacterClass(currentChar);

                if (charClass != AvaloniaEdit.Document.CharacterClass.LineTerminator && prevChar != ' ' &&
                    AvaloniaEdit.Document.TextUtilities.GetCharacterClass(prevChar) != AvaloniaEdit.Document.CharacterClass.LineTerminator)
                {
                    start = AvaloniaEdit.Document.TextUtilities.GetNextCaretPosition(Document, offset, AvaloniaEdit.Document.LogicalDirection.Backward,
                        AvaloniaEdit.Document.CaretPositioningMode.WordStart);
                }

                var end = AvaloniaEdit.Document.TextUtilities.GetNextCaretPosition(Document, start, AvaloniaEdit.Document.LogicalDirection.Forward,
                    AvaloniaEdit.Document.CaretPositioningMode.WordBorder);

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

        private void SetDebugHighlight(int line, int startColumn, int endColumn)
        {
            if (startColumn == -1 && endColumn == -1)
            {
                var docLine = Document.GetLineByNumber(line);

                var leading = AvaloniaEdit.Document.TextUtilities.GetLeadingWhitespace(Document, docLine);

                var trailing = AvaloniaEdit.Document.TextUtilities.GetTrailingWhitespace(Document, docLine);

                startColumn = Document.GetLocation(leading.EndOffset).Column;
                endColumn = Document.GetLocation(trailing.Offset).Column;
            }

            _selectedDebugLineBackgroundRenderer.SetLocation(line, startColumn, endColumn);

            Dispatcher.UIThread.Post(() =>
            {
                var viewPortLines = (int)((TextArea as IScrollable).Viewport.Height);

                if (viewPortLines < Document.LineCount)
                {
                    TextArea.ScrollToLine(line);
                }
            });
        }

        private void ClearDebugHighlight()
        {
            _selectedDebugLineBackgroundRenderer.SetLocation(-1);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            Close();

            base.OnDetachedFromVisualTree(e);
        }

        internal void Close()
        {
            if (_scopeLineBackgroundRenderer != null)
            {
                TextArea.TextView.BackgroundRenderers.Remove(_scopeLineBackgroundRenderer);
                _scopeLineBackgroundRenderer.Dispose();
                _scopeLineBackgroundRenderer = null;
            }

            if (_textColorizer != null)
            {
                TextArea.TextView.LineTransformers.Remove(_textColorizer);
                _textColorizer.Dispose();
                _textColorizer = null;
            }

            if (_diagnosticMarkersRenderer != null)
            {
                TextArea.TextView.BackgroundRenderers.Remove(_diagnosticMarkersRenderer);
                _diagnosticMarkersRenderer.Dispose();
                _diagnosticMarkersRenderer = null;
            }

            TextArea.TextEntering -= TextArea_TextEntering;
            TextArea.TextEntered -= TextArea_TextEntered;

            _intellisenseManager = null;

            _disposables.Dispose();

            TextArea.TextView.BackgroundRenderers.Clear();
            TextArea.TextView.LineTransformers.Clear();
            TextArea.LeftMargins.Clear();

            _breakpointMargin.Dispose();
            _lineNumberMargin.Dispose();
        }
    }
}
