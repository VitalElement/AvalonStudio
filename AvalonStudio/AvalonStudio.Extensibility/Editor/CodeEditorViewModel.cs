using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using ReactiveUI;
using System.Linq;

namespace AvalonStudio.Extensibility.Editor
{
    public class CodeEditorViewModel : TextEditorViewModel, IDebugLineDocumentTabViewModel, IEditor2
    {
        private DebugHighlightLocation _debugHighlight;
        private ILanguageService _languageService;

        public CodeEditorViewModel(ITextDocument document, ISourceFile file) : base (document, file)
        {
            RegisterLanguageService(file);
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

            var languageService = IoC.Get<IStudio>().LanguageServices.FirstOrDefault(
                o => o.Metadata.TargetCapabilities.Any(
                    c => contentTypeService.CapabilityAppliesToContentType(c, sourceFile.ContentType)))?.Value;

            languageService.RegisterSourceFile(this);

            LanguageService = languageService;

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

                _languageServiceDisposables = new CompositeDisposable
                {
                    Observable.FromEventPattern<DiagnosticsUpdatedEventArgs>(LanguageService, nameof(LanguageService.DiagnosticsUpdated)).ObserveOn(AvaloniaScheduler.Instance).Subscribe(args =>
                    LanguageService_DiagnosticsUpdated(args.Sender, args.EventArgs))
                };
            }

            StartBackgroundWorkers();

            Document.TextChanged += TextDocument_TextChanged;

            TextArea.TextEntering += TextArea_TextEntering;

            TextArea.TextEntered += TextArea_TextEntered;

            DoCodeAnalysisAsync().GetAwaiter();*/
        }

        // HighlightingProvider

        // CompletionProvider

        // Formatter

        // Indentation Strategy
    }
}
