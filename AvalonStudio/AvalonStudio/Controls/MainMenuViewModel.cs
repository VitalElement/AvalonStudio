namespace AvalonStudio.Controls
{
    using ReactiveUI;
    using System;

    public class MainMenuViewModel : ReactiveObject
    {
        public MainMenuViewModel()
        {
            LoadProjectCommand = ReactiveCommand.Create();

            LoadProjectCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.LoadSolution();
            });

            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.Save();
            });

            SaveAllCommand = ReactiveCommand.Create();
            SaveAllCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.SaveAll();
            });

            CleanProjectCommand = ReactiveCommand.Create();
            CleanProjectCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.Clean();
            });

            BuildProjectCommand = ReactiveCommand.Create();
            BuildProjectCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.Build();
            });

            PackagesCommand = ReactiveCommand.Create();
            PackagesCommand.Subscribe((o) =>
            {
                WorkspaceViewModel.Instance.ShowPackagesDialog();
            });

            ProjectPropertiesCommand = ReactiveCommand.Create();
            ProjectPropertiesCommand.Subscribe((o) =>
            {
                WorkspaceViewModel.Instance.ShowProjectPropertiesDialog();
            });

            NewProjectCommand = ReactiveCommand.Create();
            NewProjectCommand.Subscribe((o) =>
            {
                WorkspaceViewModel.Instance.ShowNewProjectDialog();
            });

            ExitCommand = ReactiveCommand.Create();
            ExitCommand.Subscribe((o) =>
            {
                WorkspaceViewModel.Instance.ExitApplication();
            });

            StartDebugCommand = ReactiveCommand.Create();
            StartDebugCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.StartDebugSession();
            });

            StepIntoCommand = ReactiveCommand.Create();
            StepIntoCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.DebugManager.StepInto();
            });

            StepOverCommand = ReactiveCommand.Create();
            StepOverCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.DebugManager.StepOver();
            });

            PauseCommand = ReactiveCommand.Create();
            PauseCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.DebugManager.Pause();
            });

            StopCommand = ReactiveCommand.Create();
            StopCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.DebugManager.Stop();
            });

            RestartCommand = ReactiveCommand.Create();
            RestartCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.DebugManager.Restart();
            });
        }

        public ReactiveCommand<object> NewProjectCommand { get; }
        public ReactiveCommand<object> SaveCommand { get; }
        public ReactiveCommand<object> SaveAllCommand { get; }
        public ReactiveCommand<object> LoadProjectCommand { get; }
        public ReactiveCommand<object> ExitCommand { get; }

        public ReactiveCommand<object> CleanProjectCommand { get; }
        public ReactiveCommand<object> BuildProjectCommand { get; }
        public ReactiveCommand<object> PackagesCommand { get; }
        public ReactiveCommand<object> ProjectPropertiesCommand { get; }

        public ReactiveCommand<object> StartDebugCommand { get; }
        public ReactiveCommand<object> StepOverCommand { get; }
        public ReactiveCommand<object> StepIntoCommand { get; }
        public ReactiveCommand<object> PauseCommand { get; }
        public ReactiveCommand<object> StopCommand { get; }
        public ReactiveCommand<object> RestartCommand { get; }
    }
}
