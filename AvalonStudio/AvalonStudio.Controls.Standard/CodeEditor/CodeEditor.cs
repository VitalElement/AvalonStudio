using Avalonia;
using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Rendering;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
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
using AvalonStudio.Languages.ViewModels;
using Avalonia.Input;
using AvaloniaEdit.CodeCompletion;
using Avalonia.LogicalTree;
using AvaloniaEdit.Indentation.CSharp;
using Avalonia.Controls.Primitives;
using Avalonia.Controls;
using Avalonia.Interactivity;

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

        private readonly List<IBackgroundRenderer> _languageServiceBackgroundRenderers = new List<IBackgroundRenderer>();

        private readonly List<IVisualLineTransformer> _languageServiceDocumentLineTransformers = new List<IVisualLineTransformer>();

        public IntellisenseViewModel Intellisense => _intellisense;

        private IntellisenseManager _intellisenseManager;
        private Intellisense _intellisenseControl;
        private IntellisenseViewModel _intellisense;

        private CompletionAssistantView _completionAssistantControl;
        private CompletionAssistantViewModel _completionAssistant;

        private bool _isLoaded = false;


        private readonly IShell _shell;
        private Subject<bool> _analysisTriggerEvents = new Subject<bool>();
        private readonly JobRunner _codeAnalysisRunner;
        private CancellationTokenSource _cancellationSource;

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
            _codeAnalysisRunner = new JobRunner();

            _shell = IoC.Get<IShell>();

            var breakpointMargin = new BreakPointMargin(this, IoC.Get<IDebugManager2>().Breakpoints);

            TextArea.LeftMargins.Add(breakpointMargin);

            var lineNumberMargin = new LineNumberMargin(this) { Margin = new Thickness(0, 0, 10, 0) };

            TextArea.LeftMargins.Add(lineNumberMargin);

            TextArea.TextView.BackgroundRenderers.Add(new SelectedLineBackgroundRenderer(this));

            TextArea.TextView.BackgroundRenderers.Add(new ColumnLimitBackgroundRenderer());

            Options = new AvaloniaEdit.TextEditorOptions
            {
                ConvertTabsToSpaces = true,
                IndentationSize = 4
            };

            DocumentLineTransformersProperty.Changed.Subscribe(s =>
            {
                if (s.OldValue != null)
                {
                    var list = s.OldValue as ObservableCollection<IVisualLineTransformer>;

                    list.CollectionChanged -= List_CollectionChanged;
                }

                if (s.NewValue != null)
                {
                    var list = s.NewValue as ObservableCollection<IVisualLineTransformer>;

                    list.CollectionChanged += List_CollectionChanged;
                }
            });

            _analysisTriggerEvents.Select(_ => Observable.Timer(TimeSpan.FromMilliseconds(300)).ObserveOn(AvaloniaScheduler.Instance)
            .SelectMany(o => DoCodeAnalysisAsync())).Switch().Subscribe(_ => { });

            this.GetObservable(SourceFileProperty).OfType<ISourceFile>().Subscribe(file =>
            {
                if (System.IO.File.Exists(file.Location))
                {
                    using (var fs = System.IO.File.OpenText(file.Location))
                    {
                        Document = new TextDocument(fs.ReadToEnd())
                        {
                            FileName = file.Location
                        };
                    }
                }

                _isLoaded = true;

                RegisterLanguageService(file);
            });

            TextArea.Caret.PositionChanged += (sender, e) =>
            {
                if (_intellisenseManager != null)
                {
                    var location = Document.GetLocation(CaretOffset);
                    _intellisenseManager.SetCursor(CaretOffset, location.Line, location.Column, Standard.CodeEditor.CodeEditor.UnsavedFiles.ToList(), true);
                }

                var visualLocation = TextArea.TextView.GetVisualPosition(TextArea.Caret.Position, VisualYPosition.LineBottom);
                var visualLocationTop = TextArea.TextView.GetVisualPosition(TextArea.Caret.Position, VisualYPosition.LineTop);


                var position = visualLocation - TextArea.TextView.ScrollOffset;
                position = position.Transform(TextArea.TextView.TransformToVisual(TextArea).Value);

                _intellisenseControl.SetLocation(position);
                _completionAssistantControl.SetLocation(position);
            };

            TextArea.TextEntered += (sender, e) => _intellisenseManager?.OnTextInput(e, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column);

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
                }
            };

            EventHandler<TextInputEventArgs> tunneledTextInputHandler = (send, ee) =>
            {
                if (CaretOffset > 0)
                {
                    _intellisenseManager?.OnTextInput(ee, CaretOffset, TextArea.Caret.Line, TextArea.Caret.Column);
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
                IsDirty = true;

                TriggerCodeAnalysis();
            }
        }

        public TextSegment GetSelection()
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

        public void SetSelection(TextSegment segment)
        {
            SelectionStart = segment.StartOffset;
            SelectionLength = segment.EndOffset - segment.StartOffset;
        }

        public void Comment()
        {
            if (LanguageService != null)
            {
                if (SelectionLength > 0)
                {
                    var selection = GetSelection();
                    var anchors = new TextSegmentCollection<TextSegment>(Document);
                    anchors.Add(selection);

                    CaretOffset = LanguageService.Comment(Document, selection, CaretOffset);

                    SetSelection(selection);
                }

                Focus();
            }
        }

        public void UnComment()
        {
            /*if (Model?.LanguageService != null)
            {
                var selection = GetSelection();

                if (selection != null)
                {
                    var anchors = new TextSegmentCollection<TextSegment>(TextDocument);
                    anchors.Add(selection);

                    CaretIndex = Model.LanguageService.UnComment(TextDocument, selection, CaretIndex);

                    SetSelection(selection);
                }

                Model.Focus();
            }*/
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

            await _codeAnalysisRunner.InvokeAsync(async () =>
            {
                if (LanguageService != null)
                {
                    // TODO allow interruption.
                    var result = await LanguageService.RunCodeAnalysisAsync(sourceFile, unsavedFiles, () => false);

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Diagnostics = result.Diagnostics;

                        _shell.InvalidateErrors();

                        TextArea.TextView.Redraw();
                    });
                }
            });

            return true;
        }

        private void RegisterLanguageService(ISourceFile sourceFile)
        {
            UnRegisterLanguageService();

            LanguageService = _shell.LanguageServices.FirstOrDefault(o => o.CanHandle(sourceFile));

            if (LanguageService != null)
            {
                //_shell.Instance.StatusBar.Language = LanguageService.Title;

                LanguageService.RegisterSourceFile(this, sourceFile, Document);

                _languageServiceBackgroundRenderers.AddRange(LanguageService.GetBackgroundRenderers(sourceFile));

                foreach (var backgroundRenderer in _languageServiceBackgroundRenderers)
                {
                    TextArea.TextView.BackgroundRenderers.Add(backgroundRenderer);
                }

                _languageServiceDocumentLineTransformers.AddRange(LanguageService.GetDocumentLineTransformers(sourceFile));

                foreach (var transformer in _languageServiceDocumentLineTransformers)
                {
                    TextArea.TextView.LineTransformers.Add(transformer);
                }

                _intellisenseManager = new IntellisenseManager(this, _intellisense, _completionAssistant, LanguageService, sourceFile);

                TextArea.IndentationStrategy = LanguageService.IndentationStrategy;
            }
            else
            {
                LanguageService = null;
                //ShellViewModel.Instance.StatusBar.Language = "Text";
            }

            StartBackgroundWorkers();

            Document.TextChanged += TextDocument_TextChanged;

            DoCodeAnalysisAsync().GetAwaiter();
        }

        public void UnRegisterLanguageService()
        {
            foreach (var backgroundRenderer in _languageServiceBackgroundRenderers)
            {
                TextArea.TextView.BackgroundRenderers.Remove(backgroundRenderer);
            }

            _languageServiceBackgroundRenderers.Clear();

            foreach (var transformer in _languageServiceDocumentLineTransformers)
            {
                TextArea.TextView.LineTransformers.Remove(transformer);
            }

            _languageServiceDocumentLineTransformers.Clear();

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
            AvaloniaProperty.Register<CodeEditor, int>(nameof(EditorCaretOffset));

        public int EditorCaretOffset
        {
            get { return GetValue(CaretOffsetProperty); }
            set { SetValue(CaretOffsetProperty, value); }
        }


        public static readonly StyledProperty<ObservableCollection<IVisualLineTransformer>> DocumentLineTransformersProperty =
            AvaloniaProperty.Register<CodeEditor, ObservableCollection<IVisualLineTransformer>>(nameof(DocumentLineTransformers), new ObservableCollection<IVisualLineTransformer>());

        public ObservableCollection<IVisualLineTransformer> DocumentLineTransformers
        {
            get { return GetValue(DocumentLineTransformersProperty); }
            set { SetValue(DocumentLineTransformersProperty, value); }
        }

        public static readonly StyledProperty<ObservableCollection<IBackgroundRenderer>> BackgroundRenderersProperty =
            AvaloniaProperty.Register<CodeEditor, ObservableCollection<IBackgroundRenderer>>(nameof(BackgroundRenderers), new ObservableCollection<IBackgroundRenderer>());

        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return GetValue(BackgroundRenderersProperty); }
            set { SetValue(BackgroundRenderersProperty, value); }
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
    }
}
