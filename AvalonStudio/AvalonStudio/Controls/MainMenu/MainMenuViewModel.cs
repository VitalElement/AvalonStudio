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
    using System.Threading.Tasks;
    using ViewModels;

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

                if (result != null)
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

            BuildProjectCommand = new RoutingCommand(async (args) =>
            {
                //new Thread(new ThreadStart(new Action(async () =>
                {                    
                    await Workspace.This.SolutionExplorer.Model.DefaultProject.Build(Workspace.This.Console, Workspace.This.ProcessCancellationToken);
                }//))).Start();
            });

            this.PackagesCommand = new RoutingCommand((o) =>
            {
                Workspace.This.ModalDialog = new PackageManagerDialogViewModel();
                Workspace.This.ModalDialog.ShowDialog();
            });
        }


        public ICommand LoadProjectCommand { get; private set; }
        public ICommand BuildProjectCommand { get; private set; }
        public ICommand PackagesCommand { get; private set; }
    }
}
