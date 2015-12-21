namespace AvalonStudio.Controls
{
    using Perspex.Controls;
    using Perspex.Threading;
    using Projects.VEBuild;
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
                dlg.Filters.Add(new FileDialogFilter { Name = "AvalonStudio Project", Extensions = new List<string> { "vsln" } });
                dlg.InitialFileName = string.Empty;
                dlg.InitialDirectory = "c:\\";
                var result = await dlg.ShowAsync();

                if (result != null)
                {
                    Workspace.Instance.SolutionExplorer.Model = Solution.Load(Path.GetDirectoryName(result[0]));
                }
            });

            SaveCommand = ReactiveCommand.Create();

            SaveCommand.Subscribe(_ =>
            {
                Workspace.Instance.Editor.Save();
            });

            BuildProjectCommand = ReactiveCommand.Create();
            //BuildProjectCommand.Subscribe(async _ =>
            //{
            //    //new Thread(new ThreadStart(new Action(async () =>
            //    {                    
            //        //await Workspace.Instance.SolutionExplorer.Model.StartupProject.Build(Workspace.Instance.Console, Workspace.Instance.ProcessCancellationToken);
            //    }//))).Start();
            //});

            PackagesCommand = ReactiveCommand.Create();
            PackagesCommand.Subscribe((o) =>
            {
                Workspace.Instance.ModalDialog = new PackageManagerDialogViewModel();
                Workspace.Instance.ModalDialog.ShowDialog();
            });            
        }



        public ReactiveCommand<object> SaveCommand { get; private set; }
        public ReactiveCommand<object> LoadProjectCommand { get; private set; }
        public ReactiveCommand<object> BuildProjectCommand { get; private set; }
        public ReactiveCommand<object> PackagesCommand { get; private set; }
    }
}
