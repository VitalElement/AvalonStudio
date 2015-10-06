namespace AvalonStudio
{
    using Controls.ViewModels;
    using Controls;
    using Models.Platform;
    using System.Threading.Tasks;
    using Models.PackageManager;
    using System.Threading;
    using System.Windows.Input;
    using ReactiveUI;
    using Models;

    public class Workspace : ReactiveObject
    {
        public static Workspace This = null;

        public Workspace()
        {
            AvalonStudioService.Initialise();

            MainMenu = new MainMenuViewModel();
            SolutionExplorer = new SolutionExplorerViewModel();
            Editor = new EditorViewModel();
            Console = new ConsoleViewModel();
            StatusBar = new StatusBarViewModel();

            StatusBar.LineNumber = 1;
            StatusBar.Column = 1;
            StatusBar.PlatformString = Platform.PlatformString;

            Task.Factory.StartNew(async () =>
            {
               var repo = await Repository.DownloadCatalog();

                foreach(var package in repo.Packages)
                {
                    Console.WriteLine(package.Name);
                }

               // MainMenu.LoadProjectCommand.Execute(null);
            });

            ProcessCancellationToken = new CancellationTokenSource();

            ModalDialog = new PackageManagerDialogViewModel();
        }

        public MainMenuViewModel MainMenu { get; private set; }

        public SolutionExplorerViewModel SolutionExplorer { get; private set; }

        public EditorViewModel Editor { get; private set; }

        public ConsoleViewModel Console { get; private set; }        

        public StatusBarViewModel StatusBar { get; private set; }

        public CancellationTokenSource ProcessCancellationToken { get; private set; }        

        private ModalDialogReactiveObject modalDialog;
        public ModalDialogReactiveObject ModalDialog
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
    }
}
