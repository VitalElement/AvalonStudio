namespace AvalonStudio
{
    using Controls.ViewModels;
    using Controls;
    using Models.Platform;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading;
    using ReactiveUI;
    using Models;
    using Languages;
    using Projects;
    using System.Linq;
    using Toolchains;

    [Export(typeof(Workspace))]
    public class Workspace : ReactiveObject
    {
        private readonly EditorModel editor;
        private readonly IEnumerable<ILanguageService> languageServices;
        private readonly IEnumerable<IToolChain> toolChains;

        public static Workspace Instance = null;

        [ImportingConstructor]
        public Workspace(EditorModel editor, [ImportMany] IEnumerable<ILanguageService> languageServices, [ImportMany] IEnumerable<IToolChain> toolChains)
        {
            this.editor = editor;
            this.languageServices = languageServices;
            this.toolChains = toolChains;

            AvalonStudioService.Initialise();

            MainMenu = new MainMenuViewModel();
            SolutionExplorer = new SolutionExplorerViewModel();
            Editor = new EditorViewModel(editor);
            Console = new ConsoleViewModel();
            ErrorList = new ErrorListViewModel();
            StatusBar = new StatusBarViewModel();

            StatusBar.LineNumber = 1;
            StatusBar.Column = 1;
            StatusBar.PlatformString = Platform.PlatformString;

            SolutionExplorer.SelectedItemChanged += (sender, e) =>
            {
                if (e is SourceFileViewModel)
                {
                    Editor.Model.OpenFile((e as SourceFileViewModel).Model as ISourceFile);
                }
            };

            this.editor.CodeAnalysisCompleted += (sender, e) =>
            {
                InvalidateErrors();
            };

            ProcessCancellationToken = new CancellationTokenSource();

            ModalDialog = new ModalDialogViewModelBase("Dialog");
        }

        public IEnumerable<ILanguageService> LanguageServices
        {
            get
            {
                return languageServices;
            }
        }

        public IEnumerable<IToolChain> ToolChains
        {
            get
            {
                return toolChains;
            }
        }

        public void InvalidateErrors()
        {
            var allErrors = new List<ErrorViewModel>();
            var toRemove = new List<ErrorViewModel>();


            foreach (var diagnostic in editor.CodeAnalysisResults.Diagnostics)
            {
                //if (diagnostic.Location.FileLocation.File.FileName.NormalizePath() == document.FilePath.NormalizePath())
                {
                    var error = new ErrorViewModel(diagnostic);
                    var matching = allErrors.FirstOrDefault((err) => err.IsEqual(error));

                    if (matching == null)
                    {
                        allErrors.Add(error);
                    }
                }
            }

            foreach (var error in ErrorList.Errors)
            {
                var matching = allErrors.SingleOrDefault((err) => err.IsEqual(error));

                if (matching == null)
                {
                    toRemove.Add(error);
                }
            }

            foreach (var error in toRemove)
            {
                ErrorList.Errors.Remove(error);
            }

            foreach (var error in allErrors)
            {
                var matching = ErrorList.Errors.SingleOrDefault((err) => err.IsEqual(error));

                if (matching == null)
                {
                    //hasChanged = true;
                    ErrorList.Errors.Add(error);
                }
            }
        }

        public MainMenuViewModel MainMenu { get; private set; }

        public SolutionExplorerViewModel SolutionExplorer { get; private set; }

        public EditorViewModel Editor { get; private set; }

        public ConsoleViewModel Console { get; private set; }

        public ErrorListViewModel ErrorList { get; private set; }

        public StatusBarViewModel StatusBar { get; private set; }

        public CancellationTokenSource ProcessCancellationToken { get; private set; }

        private ModalDialogViewModelBase modalDialog;
        public ModalDialogViewModelBase ModalDialog
        {
            get { return modalDialog; }
            set { modalDialog = value; this.RaisePropertyChanged(); }
        }

        public void InvalidateCodeAnalysis()
        {

        }

        private bool hideWhenModalVisibility = true;
        public bool HideWhenModalVisibility
        {
            get { return hideWhenModalVisibility; }
            set { hideWhenModalVisibility = value; this.RaisePropertyChanged(); }
        }

        public void Cleanup()
        {
            editor.ShutdownBackgroundWorkers();
        }
    }
}
