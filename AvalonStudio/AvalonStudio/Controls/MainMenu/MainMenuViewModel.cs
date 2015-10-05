namespace AvalonStudio.Controls
{
    using AvalonStudio.Models.Solutions;
    using Perspex.Controls;
    using Perspex.MVVM;
    using System;
    using System.IO;
    using System.Threading;
    using System.Windows.Input;
    using System.Linq;
    using Perspex.Threading;

    public class MainMenuViewModel : ViewModelBase
    {
        public MainMenuViewModel()
        {
            LoadProjectCommand = new RoutingCommand(async (args) =>
            {
                var dlg = new OpenFileDialog();
                dlg.InitialFileName = string.Empty;
                dlg.InitialDirectory = "c:\\";
                var result = await dlg.ShowAsync();

                if (result.Length == 1)
                {
                    new Thread (new ThreadStart(new Action (() =>
                    {
                        Workspace.This.SolutionExplorer.Model = Solution.LoadSolution(result[0]);
                        using (var fs = File.OpenText(Workspace.This.SolutionExplorer.Model.DefaultProject.Children.OfType<ProjectFile>().First().Location))
                        {
                            var content = fs.ReadToEnd();

                            Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                Workspace.This.Editor.Text = content;
                            });
                        }
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
