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
                if (ShellViewModel.Instance.CurrentPerspective == Perspective.Editor)
                {
                    if (ShellViewModel.Instance.SolutionExplorer.Model?.StartupProject != null)
                    {
                        ShellViewModel.Instance.DebugManager.StartDebug(ShellViewModel.Instance.SolutionExplorer.Model.StartupProject);
                    }
                }
                else
                {
                    ShellViewModel.Instance.DebugManager.Continue();
                }
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

        public ReactiveCommand<object> StartDebugCommand { get; }
        public ReactiveCommand<object> StepOverCommand { get; }
        public ReactiveCommand<object> StepIntoCommand { get; }
        public ReactiveCommand<object> PauseCommand { get; }
        public ReactiveCommand<object> StopCommand { get; }
        public ReactiveCommand<object> RestartCommand { get; }
    }
}
