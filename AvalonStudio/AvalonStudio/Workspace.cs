namespace AvalonStudio
{
    using Perspex.MVVM;
    using AvalonStudio.Controls.ViewModel;
    using Controls;
    using Models;

    public class Workspace : ViewModelBase
    {
        public static Workspace This = new Workspace();

        public Workspace()
        {
            MainMenu = new MainMenuViewModel();
            SolutionExplorer = new SolutionExplorerViewModel();
            Editor = new EditorViewModel();
            Console = new ConsoleViewModel();
            StatusBar = new StatusBarViewModel();

            StatusBar.LineNumber = 1;
            StatusBar.Column = 1;		
        }

        public MainMenuViewModel MainMenu { get; private set; }

        public SolutionExplorerViewModel SolutionExplorer { get; private set; }

        public EditorViewModel Editor { get; private set; }

        public ConsoleViewModel Console { get; private set; }        

        public StatusBarViewModel StatusBar { get; private set; }
    }
}
