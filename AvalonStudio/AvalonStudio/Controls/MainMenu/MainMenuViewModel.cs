namespace AvalonStudio.Controls
{
    using Perspex.MVVM;
    using System.Windows.Input;

    public class MainMenuViewModel : ViewModelBase
    {
        public MainMenuViewModel()
        {
            LoadProjectCommand = new RoutedCommand((args) =>
            {
                Perspex.Controls.CommonDialog dlg = new Perspex.Controls.CommonDialog();

                dlg.ShowAsync();
                //var ofd = new OpenFileDialog();
                //ofd.InitialDirectory = VEStudioService.DefaultProjectFolder;
                //ofd.Filter = "VEStudio Solution Files (*" + ".vesln" + ")|*" + ".vesln";

                //if (ofd.ShowDialog() == DialogResult.OK)
                //{
                //    //ResetWorkspace();

                //    var solution = Solution.LoadSolution(ofd.FileName);

                //    Workspace.This.SolutionExplorer.Model = solution;
                //}
            });
        }


        public ICommand LoadProjectCommand
        {
            get; set;
        }
    }
}
