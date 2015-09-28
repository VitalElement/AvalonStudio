namespace AvalonStudio.Controls
{
    using Perspex.MVVM;
    using System.Windows.Input;
    using AvalonStudio.Models.Solutions;
    using System.Threading;
    using System;
	using AvalonStudio.Models;

    public class MainMenuViewModel : ViewModelBase
    {
        public MainMenuViewModel()
        {
            LoadProjectCommand = new RoutedCommand(async (args) =>
            {
               // Perspex.Controls.CommonDialog dlg = new Perspex.Controls.CommonDialog();

                //var result = await dlg.ShowAsync();


                //if (result.Length == 1)
                {
                    new Thread (new ThreadStart(new Action (async () =>
                    {
                        //var solution = Solution.LoadSolution(result[0]);
								AvalonStudioSettings.This.TestInterface (Workspace.This.Console);
                        //await solution.DefaultProject.Build(Workspace.This.Console, new System.Threading.CancellationTokenSource());
                    }))).Start();
                }              
            });
        }


        public ICommand LoadProjectCommand
        {
            get; set;
        }
    }
}
