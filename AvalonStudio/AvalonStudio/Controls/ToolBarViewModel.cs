namespace AvalonStudio.Controls
{
    using MVVM;
    using System;
    using ReactiveUI;

    public class ToolBarViewModel : ViewModel
    {
        public ToolBarViewModel()
        {
            StartDebugCommand = ReactiveCommand.Create();
            StartDebugCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.StartDebugSession();
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

            BuildCommand = ReactiveCommand.Create();
            BuildCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.Build();
            });

            CleanCommand = ReactiveCommand.Create();
            CleanCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.Clean();
            });
        }

        public ReactiveCommand<object> StartDebugCommand { get; }
        public ReactiveCommand<object> StepOverCommand { get; }
        public ReactiveCommand<object> StepIntoCommand { get; }
        public ReactiveCommand<object> PauseCommand { get; }
        public ReactiveCommand<object> StopCommand { get; }
        public ReactiveCommand<object> RestartCommand { get; }
        public ReactiveCommand<object> BuildCommand { get; }
        public ReactiveCommand<object> CleanCommand { get; }
    }
}
