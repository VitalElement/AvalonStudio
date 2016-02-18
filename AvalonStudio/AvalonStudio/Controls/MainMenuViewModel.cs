﻿namespace AvalonStudio.Controls
{
    using Extensibility.Platform;
    using Perspex.Controls;
    using Perspex.Threading;
    using Projects;
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

            LoadProjectCommand.Subscribe(async _ =>
            {
                var dlg = new OpenFileDialog();
                dlg.Title = "Open Solution";
                dlg.Filters.Add(new FileDialogFilter { Name = "AvalonStudio Solution", Extensions = new List<string> { Solution.Extension } });
                dlg.InitialFileName = string.Empty;
                dlg.InitialDirectory = Platform.ProjectDirectory;
                var result = await dlg.ShowAsync();

                if (result != null)
                {
                    WorkspaceViewModel.Instance.SolutionExplorer.Model = Solution.Load(result[0]);
                }
            });

            SaveCommand = ReactiveCommand.Create();

            SaveCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.Editor.Save();
            });

            CleanProjectCommand = ReactiveCommand.Create();
            CleanProjectCommand.Subscribe(_ =>
            {
                new Thread(new ThreadStart(new Action(async () =>
                {
                    await WorkspaceViewModel.Instance.SolutionExplorer.Model.StartupProject.ToolChain.Clean(WorkspaceViewModel.Instance.Console, WorkspaceViewModel.Instance.SolutionExplorer.Model.StartupProject);
                }))).Start();
            });

            BuildProjectCommand = ReactiveCommand.Create();
            BuildProjectCommand.Subscribe(_ =>
            {
                new Thread(new ThreadStart(new Action(async () =>
                {
                    if (WorkspaceViewModel.Instance.SolutionExplorer.Model != null)
                    {
                        if (WorkspaceViewModel.Instance.SolutionExplorer.Model.StartupProject != null)
                        {
                            if (WorkspaceViewModel.Instance.SolutionExplorer.Model.StartupProject.ToolChain != null)
                            {
                                await WorkspaceViewModel.Instance.SolutionExplorer.Model.StartupProject.ToolChain.Build(WorkspaceViewModel.Instance.Console, WorkspaceViewModel.Instance.SolutionExplorer.Model.StartupProject);
                            }
                            else
                            {
                                WorkspaceViewModel.Instance.Console.WriteLine("No toolchain selected for project.");
                            }
                        }
                        else
                        {
                            WorkspaceViewModel.Instance.Console.WriteLine("No Startup Project defined.");
                        }
                    }
                    else
                    {
                        WorkspaceViewModel.Instance.Console.WriteLine("No project loaded.");
                    }
                }))).Start();
            });

            PackagesCommand = ReactiveCommand.Create();
            PackagesCommand.Subscribe((o) =>
            {
                WorkspaceViewModel.Instance.ModalDialog = new PackageManagerDialogViewModel();
                WorkspaceViewModel.Instance.ModalDialog.ShowDialog();
            });

            ProjectPropertiesCommand = ReactiveCommand.Create();
            ProjectPropertiesCommand.Subscribe((o) =>
            {
                WorkspaceViewModel.Instance.ModalDialog = new ProjectConfigurationDialogViewModel(WorkspaceViewModel.Instance.SolutionExplorer.SelectedProject, () => { });
                WorkspaceViewModel.Instance.ModalDialog.ShowDialog();
            });

            NewProjectCommand = ReactiveCommand.Create();
            NewProjectCommand.Subscribe((o) =>
            {
                WorkspaceViewModel.Instance.ModalDialog = new NewProjectDialogViewModel();
                WorkspaceViewModel.Instance.ModalDialog.ShowDialog();
            });

            ExitCommand = ReactiveCommand.Create();
            ExitCommand.Subscribe((o) =>
            {
                Environment.Exit(1);
            });
        }

        public ReactiveCommand<object> NewProjectCommand { get; private set; }
        public ReactiveCommand<object> SaveCommand { get; private set; }
        public ReactiveCommand<object> LoadProjectCommand { get; private set; }
        public ReactiveCommand<object> ExitCommand { get; }

        public ReactiveCommand<object> CleanProjectCommand { get; private set; }
        public ReactiveCommand<object> BuildProjectCommand { get; private set; }
        public ReactiveCommand<object> PackagesCommand { get; private set; }
        public ReactiveCommand<object> ProjectPropertiesCommand { get; private set; }
    }
}
