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

    [Export(typeof(Workspace))]
    public class Workspace : ReactiveObject
    {
        private readonly EditorModel editor;
        private readonly IEnumerable<ILanguageService> languageServices;
        public static Workspace Instance = null;

        [ImportingConstructor]
        public Workspace(EditorModel editor, [ImportMany] IEnumerable<ILanguageService> languageServices)
        {
            this.editor = editor;
            this.languageServices = languageServices;
            AvalonStudioService.Initialise();

            MainMenu = new MainMenuViewModel();
            SolutionExplorer = new SolutionExplorerViewModel();
            Editor = new EditorViewModel(editor);
            Console = new ConsoleViewModel();
            StatusBar = new StatusBarViewModel();

            StatusBar.LineNumber = 1;
            StatusBar.Column = 1;
            StatusBar.PlatformString = Platform.PlatformString;

            SolutionExplorer.SelectedItemChanged += (sender, e) =>
            {
                //try {
                if (e is ProjectFileViewModel)
                {
                    Editor.Model.OpenFile((e as ProjectFileViewModel).Model as ISourceFile);
                }
                //} catch(Exception) {

                //}
            };

            //Task.Factory.StartNew(async () =>
            //{
            //   var repo = await Repository.DownloadCatalog();

            //    foreach(var package in repo.Packages)
            //    {
            //        Console.WriteLine(package.Name);
            //    }

            //   // MainMenu.LoadProjectCommand.Execute(null);
            //});

            ProcessCancellationToken = new CancellationTokenSource();

            ModalDialog = new ModalDialogViewModelBase("Dialog");// new PackageManagerDialogViewModel();
        }

        public MainMenuViewModel MainMenu { get; private set; }

        public SolutionExplorerViewModel SolutionExplorer { get; private set; }

        public EditorViewModel Editor { get; private set; }

        public ConsoleViewModel Console { get; private set; }

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
