namespace AvalonStudio.Controls
{
    using Perspex.MVVM;
    using System.Windows.Input;
	using AvalonStudio.Models.Solutions;

    public class MainMenuViewModel : ViewModelBase
    {
        public MainMenuViewModel()
        {
            LoadProjectCommand = new RoutedCommand(async (args) =>
            {
                Perspex.Controls.CommonDialog dlg = new Perspex.Controls.CommonDialog();

					var result = await dlg.ShowAsync();


					if(result.Length == 1)
					{
						var solution = Solution.LoadSolution (result[0]);

						await solution.DefaultProject.Build(Workspace.This.Console, new System.Threading.CancellationTokenSource());
					}



				
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
