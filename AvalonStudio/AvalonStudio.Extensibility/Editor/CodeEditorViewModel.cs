using Avalonia.Threading;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
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

                        var toRemove = _highlights.Where(h => h.tag.Equals(this)).ToList();

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
            var contentTypeService = ContentTypeServiceInstance.Instance;

            var languageServiceProvider = IoC.Get<IStudio>().LanguageServiceProviders.FirstOrDefault(
                o => o.Metadata.TargetCapabilities.Any(
                    c => contentTypeService.CapabilityAppliesToContentType(c, sourceFile.ContentType)))?.Value;

            LanguageService = languageServiceProvider.CreateLanguageService();
            LanguageService.RegisterSourceFile(this);

            _languageServiceDisposables = new CompositeDisposable
            {
                Observable.FromEventPattern<DiagnosticsUpdatedEventArgs>(IoC.Get<IErrorList>(), nameof(IErrorList.DiagnosticsUpdated)).Subscribe(args =>
                LanguageService_DiagnosticsUpdated(args.Sender, args.EventArgs))
            };

            /*SyntaxHighlighting = CustomHighlightingManager.Instance.GetDefinition(sourceFile.ContentType);}*/

            StartBackgroundWorkers();

            DoCodeAnalysisAsync().GetAwaiter();
        }

        private void LanguageService_DiagnosticsUpdated(object sender, DiagnosticsUpdatedEventArgs e)
        {
            switch (e.Kind)
            {
                case DiagnosticsUpdatedKind.DiagnosticsRemoved:
                    var toRemove = _diagnostics.Where(x => x.tag.Equals(e.Tag)).ToList();

                    foreach (var diagnostic in toRemove)
                    {
                        _diagnostics.Remove(diagnostic);
                    }

                    var highlightsToRemove = _highlights.Where(h => h.tag.Equals(e.Tag)).ToList();

                    foreach (var highlightData in highlightsToRemove)
                    {
                        _highlights.Remove(highlightData);
                    }
                    break;

                case DiagnosticsUpdatedKind.DiagnosticsCreated:
                    if (e.Source == DiagnosticSource.Analysis)
                    {
                        _diagnostics.Add((e.Tag, e.Diagnostics));

                        if (e.DiagnosticHighlights != null)
                        {
                            _highlights.Add((e.Tag, e.DiagnosticHighlights));
                        }
                    }
                    break;
            }
        }
    }
}
