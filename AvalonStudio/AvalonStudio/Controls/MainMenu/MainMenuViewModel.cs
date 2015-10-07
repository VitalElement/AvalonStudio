namespace AvalonStudio.Controls
{
    using AvalonStudio.Models.Solutions;
    using Perspex.Controls;
    using Perspex.Threading;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using ViewModels;

    public class MainMenuViewModel : ReactiveObject
    {
        public MainMenuViewModel()
        {            
            LoadProjectCommand = ReactiveCommand.Create();

            LoadProjectCommand.Subscribe(async _=>
            {
                var dlg = new OpenFileDialog();
                dlg.Title = "Open Project";
                dlg.Filters.Add(new FileDialogFilter { Name = "AvalonStudio Project", Extensions = new List<string> { "vesln" } });
                dlg.InitialFileName = string.Empty;
                dlg.InitialDirectory = "c:\\";
                var result = await dlg.ShowAsync();

                if (result != null)
                {
                    new Thread(new ThreadStart(new Action(() =>
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

            BuildProjectCommand = ReactiveCommand.Create();
            BuildProjectCommand.Subscribe(async _ =>
            {
                //new Thread(new ThreadStart(new Action(async () =>
                {                    
                    await Workspace.This.SolutionExplorer.Model.DefaultProject.Build(Workspace.This.Console, Workspace.This.ProcessCancellationToken);
                }//))).Start();
            });

            PackagesCommand = ReactiveCommand.Create();
            PackagesCommand.Subscribe((o) =>
            {
                Workspace.This.ModalDialog = new PackageManagerDialogViewModel();
                Workspace.This.ModalDialog.ShowDialog();
            });
        }


        public ReactiveCommand<object> LoadProjectCommand { get; private set; }
        public ReactiveCommand<object> BuildProjectCommand { get; private set; }
        public ReactiveCommand<object> PackagesCommand { get; private set; }
    }
}
