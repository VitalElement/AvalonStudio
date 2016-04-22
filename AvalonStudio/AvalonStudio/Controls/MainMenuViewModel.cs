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
                ShellViewModel.Instance.LoadSolution();
            });

            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.Save();
            });

            SaveAllCommand = ReactiveCommand.Create();
            SaveAllCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.SaveAll();
            });

            CleanProjectCommand = ReactiveCommand.Create();
            CleanProjectCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.Clean();
            });

            BuildProjectCommand = ReactiveCommand.Create();
            BuildProjectCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.Build();
            });

            PackagesCommand = ReactiveCommand.Create();
            PackagesCommand.Subscribe((o) =>
            {
                ShellViewModel.Instance.ShowPackagesDialog();
            });

            ProjectPropertiesCommand = ReactiveCommand.Create();
            ProjectPropertiesCommand.Subscribe((o) =>
            {
                ShellViewModel.Instance.ShowProjectPropertiesDialog();
            });

            NewProjectCommand = ReactiveCommand.Create();
            NewProjectCommand.Subscribe((o) =>
            {
                ShellViewModel.Instance.ShowNewProjectDialog();
            });

            ExitCommand = ReactiveCommand.Create();
            ExitCommand.Subscribe((o) =>
            {
                ShellViewModel.Instance.ExitApplication();
            });

            StartDebugCommand = ReactiveCommand.Create();
            StartDebugCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.Debug();
            });

            StepIntoCommand = ReactiveCommand.Create();
            StepIntoCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.DebugManager.StepInto();
            });

            StepOverCommand = ReactiveCommand.Create();
            StepOverCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.DebugManager.StepOver();
            });

            PauseCommand = ReactiveCommand.Create();
            PauseCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.DebugManager.Pause();
            });

            StopCommand = ReactiveCommand.Create();
            StopCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.DebugManager.Stop();
            });

            RestartCommand = ReactiveCommand.Create();
            RestartCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.DebugManager.Restart();
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
