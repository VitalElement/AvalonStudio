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
                if (WorkspaceViewModel.Instance.CurrentPerspective == Perspective.Editor)
                {
                    if (WorkspaceViewModel.Instance.SolutionExplorer.Model?.StartupProject != null)
                    {
                        WorkspaceViewModel.Instance.DebugManager.StartDebug(WorkspaceViewModel.Instance.SolutionExplorer.Model.StartupProject);
                    }
                }
                else
                {
                    WorkspaceViewModel.Instance.DebugManager.Continue();
                }
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

        public ReactiveCommand<object> StartDebugCommand { get; }
        public ReactiveCommand<object> StepOverCommand { get; }
        public ReactiveCommand<object> StepIntoCommand { get; }
        public ReactiveCommand<object> PauseCommand { get; }
        public ReactiveCommand<object> StopCommand { get; }
        public ReactiveCommand<object> RestartCommand { get; }
    }
}
