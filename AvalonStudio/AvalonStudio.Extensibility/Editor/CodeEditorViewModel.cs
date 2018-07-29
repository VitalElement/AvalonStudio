using Avalonia.Threading;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Editor
{
    public class CodeEditorViewModel : TextEditorViewModel, IDebugLineDocumentTabViewModel, ICodeEditor
    {
        private DebugHighlightLocation _debugHighlight;
        private ILanguageService _languageService;
        private Subject<bool> _analysisTriggerEvents = new Subject<bool>();
        private readonly JobRunner _codeAnalysisRunner;
        private CancellationTokenSource _cancellationSource;
        private bool _isAnalyzing;
        private ObservableCollection<(object tag, SyntaxHighlightDataList)> _highlights;
        private ObservableCollection<(object tag, IEnumerable<Diagnostic>)> _diagnostics;
        private IEnumerable<IndexEntry> _codeIndex;
        private int _lastLineNumber;
        private CompositeDisposable _languageServiceDisposables;

        public CodeEditorViewModel(ITextDocument document, ISourceFile file) : base(document, file)
        {
            _lastLineNumber = -1;

            _highlights = new ObservableCollection<(object tag, SyntaxHighlightDataList)>();
            _diagnostics = new ObservableCollection<(object tag, IEnumerable<Diagnostic>)>();

            _codeAnalysisRunner = new JobRunner(1);

            RegisterLanguageService(file);

            _analysisTriggerEvents.Throttle(TimeSpan.FromMilliseconds(300)).ObserveOn(AvaloniaScheduler.Instance).Subscribe(async _ =>
            {
                await DoCodeAnalysisAsync();
            });

            this.WhenAnyValue(x => x.Line).Subscribe(lineNumber =>
            {
                if (lineNumber != _lastLineNumber && lineNumber > 0)
                {
                    var line = Document.Lines[Line];

                    var text = Document.GetText(line);

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        if (LanguageService?.InputHelpers != null)
                        {
                            foreach (var helper in LanguageService.InputHelpers)
                            {
                                helper.CaretMovedToEmptyLine(this);
                            }
                        }
                    }

                    _lastLineNumber = lineNumber;
                }
            });
        }

        public DebugHighlightLocation DebugHighlight
        {
            get { return _debugHighlight; }
            set { this.RaiseAndSetIfChanged(ref _debugHighlight, value); }
        }

        public ILanguageService LanguageService
        {
            get { return _languageService; }
            set { this.RaiseAndSetIfChanged(ref _languageService, value); }
        }

        public ObservableCollection<(object tag, SyntaxHighlightDataList highlights)> Highlights
        {
            get { return _highlights; }
            set { this.RaiseAndSetIfChanged(ref _highlights, value); }
        }

        public ObservableCollection<(object tag, IEnumerable<Diagnostic> diagnostics)> Diagnostics
        {
            get => _diagnostics;
            set => this.RaiseAndSetIfChanged(ref _diagnostics, value);
        }

        public IEnumerable<IndexEntry> CodeIndex
        {
            get { return _codeIndex; }
            set { this.RaiseAndSetIfChanged(ref _codeIndex, value); }
        }

        public override bool OnTextEntered(string text)
        {
            bool handled = false;

            if (LanguageService?.InputHelpers != null)
            {
                foreach (var helper in LanguageService.InputHelpers)
                {
                    if (helper.AfterTextInput(this, text))
                    {
                        handled = true;
                    }
                }
            }

            return handled;
        }

        public override bool OnBeforeTextEntered(string text)
        {
            bool handled = false;

            if (LanguageService?.InputHelpers != null)
            {
                foreach (var helper in LanguageService.InputHelpers)
                {
                    if (helper.BeforeTextInput(this, text))
                    {
                        handled = true;
                    }
                }
            }

            return handled;
        }

        public override void OnTextChanged()
        {
            base.OnTextChanged();

            if (!IsReadOnly)
            {
                TriggerCodeAnalysis();
            }
        }

        public override void Save()
        {
            try
            {
                FormatAll();
            }
            catch (Exception)
            {

            }

            base.Save();
        }

        public void FormatAll()
        {
            if (LanguageService != null)
            {
                if (GlobalSettings.Settings.GetSettings<EditorSettings>().AutoFormat)
                {
                    var caretOffset = LanguageService.Format(this, 0, (uint)Document.TextLength, Offset);

                    // some language services manually set the caret themselves and return -1 to indicate this.
                    if (caretOffset >= 0)
                    {
                        Offset = caretOffset;
                    }

                    Focus();
                }
            }
        }

        public void Comment()
        {

        }

        public void Uncomment()
        {

        }

        public override void IndentLine(int lineNumber)
        {
            base.IndentLine(lineNumber);
        }

        private void TriggerCodeAnalysis()
        {
            _analysisTriggerEvents.OnNext(true);
        }

        private async Task<bool> DoCodeAnalysisAsync()
        {
            var unsavedFiles = IoC.Get<IShell>().Documents.OfType<ITextDocumentTabViewModel>()
                .Where(x => x.IsDirty)
                .Select(x => new UnsavedFile(x.SourceFile.FilePath, x.Document.Text))
                .ToList();

            await _codeAnalysisRunner.InvokeAsync(async () =>
            {
                if (LanguageService != null)
                {
                    var result = await LanguageService.RunCodeAnalysisAsync(this, unsavedFiles, () => false);

                    Dispatcher.UIThread.Post(() =>
                    {
                        CodeIndex = result.IndexItems;

                        var toRemove = _highlights.Where(h => h.tag == this).ToList();

                        foreach (var highlightData in toRemove)
                        {
                            _highlights.Remove(highlightData);
                        }

                        _highlights.Add((this, result.SyntaxHighlightingData));
                    });
                }
            });

            return true;
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

        private void RegisterLanguageService(ISourceFile sourceFile)
        {
            //UnRegisterLanguageService();

            /*if (sourceFile.Project?.Solution != null)
            {
                _snippetManager.InitialiseSnippetsForSolution(sourceFile.Project.Solution);
            }

            if (sourceFile.Project != null)
            {
                _snippetManager.InitialiseSnippetsForProject(sourceFile.Project);
            }*/

            var contentTypeService = ContentTypeServiceInstance.Instance;

            var languageServiceProvider = IoC.Get<IStudio>().LanguageServiceProviders.FirstOrDefault(
                o => o.Metadata.TargetCapabilities.Any(
                    c => contentTypeService.CapabilityAppliesToContentType(c, sourceFile.ContentType)))?.Value;

            LanguageService = languageServiceProvider.CreateLanguageService();
            LanguageService.RegisterSourceFile(this);

            _languageServiceDisposables = new CompositeDisposable
            {
                Observable.FromEventPattern<DiagnosticsUpdatedEventArgs>(LanguageService, nameof(LanguageService.DiagnosticsUpdated)).ObserveOn(AvaloniaScheduler.Instance).Subscribe(args =>
                LanguageService_DiagnosticsUpdated(args.Sender, args.EventArgs))
            };


            /*SyntaxHighlighting = CustomHighlightingManager.Instance.GetDefinition(sourceFile.ContentType);

            if (LanguageService != null)
            {
                LanguageService.RegisterSourceFile(DocumentAccessor);

                _diagnosticMarkersRenderer = new TextMarkerService(Document);

                _scopeLineBackgroundRenderer = new ScopeLineBackgroundRenderer(Document);

                _contextActionsRenderer = new ContextActionsRenderer(this, _diagnosticMarkersRenderer);
                TextArea.LeftMargins.Add(_contextActionsRenderer);

                foreach (var contextActionProvider in LanguageService.GetContextActionProviders(DocumentAccessor))
                {
                    _contextActionsRenderer.Providers.Add(contextActionProvider);
                }

                TextArea.TextView.BackgroundRenderers.Add(_scopeLineBackgroundRenderer);
                TextArea.TextView.BackgroundRenderers.Add(_diagnosticMarkersRenderer);


                _intellisenseManager = new IntellisenseManager(DocumentAccessor, _intellisense, _completionAssistant, LanguageService, sourceFile, offset =>
                {
                    var location = new TextViewPosition(Document.GetLocation(offset));

                    var visualLocation = TextArea.TextView.GetVisualPosition(location, VisualYPosition.LineBottom);
                    var visualLocationTop = TextArea.TextView.GetVisualPosition(location, VisualYPosition.LineTop);

                    var position = visualLocation - TextArea.TextView.ScrollOffset;
                    position = position.Transform(TextArea.TextView.TransformToVisual(TextArea).Value);

                    _completionAssistantControl.SetLocation(position);
                });

                _disposables.Add(_intellisenseManager);

                TextArea.IndentationStrategy = LanguageService.IndentationStrategy;

                if (TextArea.IndentationStrategy == null)
                {
                    TextArea.IndentationStrategy = new DefaultIndentationStrategy();
                }

                
            }*/

            StartBackgroundWorkers();

            /*Document.TextChanged += TextDocument_TextChanged;

            TextArea.TextEntering += TextArea_TextEntering;

            TextArea.TextEntered += TextArea_TextEntered;*/

            DoCodeAnalysisAsync().GetAwaiter();
        }

        private void LanguageService_DiagnosticsUpdated(object sender, DiagnosticsUpdatedEventArgs e)
        {
            if (e.AssociatedSourceFile == SourceFile)
            {
                var toRemove = _diagnostics.Where(x => x.tag == e.Tag).ToList();

                foreach (var diagnostic in toRemove)
                {
                    _diagnostics.Remove(diagnostic);
                }

                var highlightsToRemove = _highlights.Where(h => h.tag == SourceFile).ToList();

                foreach (var highlightData in highlightsToRemove)
                {
                    _highlights.Remove(highlightData);
                }

                if (e.Kind == DiagnosticsUpdatedKind.DiagnosticsCreated)
                {
                    _diagnostics.Add((e.Tag, e.Diagnostics));

                    if (e.DiagnosticHighlights != null)
                    {
                        _highlights.Add((SourceFile, e.DiagnosticHighlights));
                    }
                }

                // TODO post to error list ??? how
                //IoC.Get<IErrorList>().UpdateDiagnostics(e);
            }
        }

        // HighlightingProvider

        // CompletionProvider

        // Formatter

        // Indentation Strategy
    }
}
