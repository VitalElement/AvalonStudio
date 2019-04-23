using Avalonia.Threading;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Languages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
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
        private CompositeDisposable _languageServiceDisposables;
        private string _renameText;
        private bool _inRenameMode;

        public CodeEditorViewModel(ITextDocument document, ISourceFile file) : base(document, file)
        {
            _highlights = new ObservableCollection<(object tag, SyntaxHighlightDataList)>();
            _diagnostics = new ObservableCollection<(object tag, IEnumerable<Diagnostic>)>();

            _codeAnalysisRunner = new JobRunner(1);

            RegisterLanguageService(file);

            _analysisTriggerEvents.Throttle(TimeSpan.FromMilliseconds(300)).ObserveOn(AvaloniaScheduler.Instance).Subscribe(async _ =>
            {
                await DoCodeAnalysisAsync();
            });

            RenameSymbolCommand = ReactiveCommand.Create(() =>
            {
                InRenameMode = true;
            });

            GotoDefinitionCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var definition = await LanguageService?.GotoDefinition(Offset);

                var studio = IoC.Get<IStudio>();

                if (definition.MetaDataFile == null)
                {
                    var definedDocument = studio.CurrentSolution.FindFile(definition.FileName);

                    if (definedDocument != null)
                    {
                        await studio.OpenDocumentAsync(definedDocument, definition.Line, definition.Column, definition.Column, selectLine: true, focus: true);
                    }
                }
                else
                {
                    await studio.OpenDocumentAsync(definition.MetaDataFile, definition.Line, definition.Column, definition.Column, selectLine: true, focus: true);
                }
            });

            this.WhenAnyValue(x => x.RenameText).Subscribe(async text =>
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    var renameInfo = await LanguageService.RenameSymbol(text);

                    await ApplyRenameAsync(renameInfo);

                    InRenameMode = false;
                }

                RenameText = "";
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

        public bool InRenameMode
        {
            get { return _inRenameMode; }
            set { this.RaiseAndSetIfChanged(ref _inRenameMode, value); }
        }

        public string RenameText
        {
            get { return _renameText; }
            set { this.RaiseAndSetIfChanged(ref _renameText, value); }
        }

        public ReactiveCommand<Unit, Unit> RenameSymbolCommand { get; }

        public ReactiveCommand<Unit, Unit> GotoDefinitionCommand { get; }

        public override bool OnClose()
        {
            LanguageService?.UnregisterEditor();
            return base.OnClose();
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
                    var caretOffset = LanguageService.Format(0, (uint)Document.TextLength, Offset);

                    // some language services manually set the caret themselves and return -1 to indicate this.
                    if (caretOffset >= 0)
                    {
                        Offset = caretOffset;
                    }

                    Focus();
                }
            }
        }

        private void ModifySelectedLines(Action<int, int> action)
        {
            var startLine = Document.GetLocation(Offset).Line;
            var endLine = startLine;

            if (Selection != null && Selection.Length > 0)
            {
                startLine = Document.GetLocation(Selection.Offset).Line;
                endLine = Document.GetLocation(Selection.EndOffset).Line;
            }

            action(startLine, endLine);

            Focus();
        }

        public void Comment()
        {
            ModifySelectedLines((start, end) =>
            {
                LanguageService.Comment(start, end, Offset);
            });
        }

        public void Uncomment()
        {
            ModifySelectedLines((start, end) =>
            {
                LanguageService.UnComment(start, end, Offset);
            });
        }

        public override void IndentLine(int lineNumber)
        {
            base.IndentLine(lineNumber);
        }

        public override async Task<object> GetToolTipContentAsync(int offset)
        {
            if (LanguageService != null)
            {
                var unsavedFiles = IoC.Get<IShell>().Documents.OfType<ITextDocumentTabViewModel>()
                .Where(x => x.IsDirty)
                .Select(x => new UnsavedFile(x.SourceFile.FilePath, x.Document.Text))
                .ToList();

                var quickinfo = await LanguageService.QuickInfo(unsavedFiles, offset);

                if (quickinfo != null && !string.IsNullOrWhiteSpace(quickinfo.Text.Text))
                {
                    return new QuickInfoViewModel(quickinfo);
                }
            }

            return await base.GetToolTipContentAsync(offset);
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
                    var result = await LanguageService.RunCodeAnalysisAsync(unsavedFiles, () => false);

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

            if (LanguageService != null)
            {
                LanguageService.RegisterEditor(this);

                _languageServiceDisposables = new CompositeDisposable
                {
                    Observable.FromEventPattern<DiagnosticsUpdatedEventArgs>(IoC.Get<IErrorList>(), nameof(IErrorList.DiagnosticsUpdated)).Subscribe(args =>
                    LanguageService_DiagnosticsUpdated(args.Sender, args.EventArgs))
                };

                if (LanguageService.InputHelpers != null)
                {
                    foreach (var helper in LanguageService.InputHelpers)
                    {
                        InputHelpers.Add(helper);
                    }
                }

                StartBackgroundWorkers();

                DoCodeAnalysisAsync().GetAwaiter();
            }
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
                    if (e.Source == DiagnosticSourceKind.Analysis && e.FilePath.CompareFilePath(SourceFile.FilePath) == 0)
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

        public async Task ApplyRenameAsync(IEnumerable<SymbolRenameInfo> renameOperation)
        {
            var studio = IoC.Get<IStudio>();

            foreach (var location in renameOperation)
            {
                var currentTab = studio.GetEditor(location.FileName);

                ITextDocument document = null;

                if (currentTab != null)
                {
                    document = currentTab.Document;
                }
                else
                {
                    document = await studio.CreateDocumentAsync(location.FileName);
                }

                if (document != null)
                {
                    using (document.RunUpdate())
                    {
                        foreach (var change in location.Changes)
                        {
                            var start = document.GetOffset(change.StartLine, change.StartColumn);
                            var end = document.GetOffset(change.EndLine, change.EndColumn);

                            document.Replace(start, end - start, change.NewText);
                        }
                    }

                    File.WriteAllText(location.FileName, document.Text);
                }
            }
        }
    }
}
