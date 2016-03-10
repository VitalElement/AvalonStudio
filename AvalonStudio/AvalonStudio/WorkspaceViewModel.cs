namespace AvalonStudio
{
    using Controls;
    using Controls.ViewModels;
    using Debugging;
    using Extensibility;
    using Platform;
    using Languages;
    using MVVM;
    using Projects;
    using ReactiveUI;
    using Repositories;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading;
    using Toolchains;
    using Utils;
    public enum Perspective
    {
        Editor,
        Debug
    }    

    [Export(typeof(WorkspaceViewModel))]
    public class WorkspaceViewModel : ViewModel<Workspace>
    {
        private readonly EditorModel editor;
        public static WorkspaceViewModel Instance = null;

        [ImportingConstructor]
        public WorkspaceViewModel(EditorModel editor, [Import] Workspace workspace) : base(workspace)
        {
            this.editor = editor;

            MainMenu = new MainMenuViewModel();
            SolutionExplorer = new SolutionExplorerViewModel();
            Editor = new EditorViewModel(editor);
            Console = new ConsoleViewModel();
            ErrorList = new ErrorListViewModel();
            StatusBar = new StatusBarViewModel();

            StatusBar.LineNumber = 1;
            StatusBar.Column = 1;
            StatusBar.PlatformString = Platform.Platform.PlatformString;

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

            DebugManager = new DebugManager();
            CurrentPerspective = Perspective.Editor;
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

        public DebugManager DebugManager { get; private set; }

        public MainMenuViewModel MainMenu { get; private set; }

        public SolutionExplorerViewModel SolutionExplorer { get; private set; }

        public EditorViewModel Editor { get; private set; }

        public IConsole Console { get; private set; }

        public ErrorListViewModel ErrorList { get; private set; }

        public StatusBarViewModel StatusBar { get; private set; }

        public CancellationTokenSource ProcessCancellationToken { get; private set; }

        private Perspective currentPerspective;
        public Perspective CurrentPerspective
        {
            get { return currentPerspective; }
            set
            {
                this.RaiseAndSetIfChanged(ref currentPerspective, value);

                switch(value)
                {
                    case Perspective.Editor:
                        break;

                    case Perspective.Debug:
                        // TODO close intellisense, and tooltips.
                        // disable documents, get rid of error list, solution explorer, etc.    (isreadonly)   
                        break;
                }
            }
        }

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
