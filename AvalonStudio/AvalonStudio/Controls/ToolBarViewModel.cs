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

            CommentCommand = ReactiveCommand.Create();
            CommentCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.SelectedDocument?.Comment();
            });

            UnCommentCommand = ReactiveCommand.Create();
            UnCommentCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.SelectedDocument?.UnComment();
            });

            UndoCommand = ReactiveCommand.Create();
            UndoCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.SelectedDocument?.Undo();
            });

            RedoCommand = ReactiveCommand.Create();
            RedoCommand.Subscribe(_ => 
            {
                ShellViewModel.Instance.SelectedDocument?.Redo();
            });

            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe(_ =>
            {
                //ShellViewModel.Instance.Save();
                GC.Collect();
            });

            SaveAllCommand = ReactiveCommand.Create();
            SaveAllCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.SaveAll();
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
        public ReactiveCommand<object> CommentCommand { get; }
        public ReactiveCommand<object> UnCommentCommand { get; }
        public ReactiveCommand<object> UndoCommand { get; }
        public ReactiveCommand<object> RedoCommand { get; }
        public ReactiveCommand<object> SaveCommand { get; }
        public ReactiveCommand<object> SaveAllCommand { get; }
    }
}
