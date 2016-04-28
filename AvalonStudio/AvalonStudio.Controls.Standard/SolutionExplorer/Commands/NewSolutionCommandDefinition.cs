namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using Extensibility;
    using ReactiveUI;
    using Shell;
    using System;
    using System.Windows.Input;

    [CommandDefinition]
    class NewSolutionCommandDefinition : CommandDefinition
    {
        public NewSolutionCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                IShell shell = IoC.Get<IShell>();
                shell.ModalDialog = new NewProjectDialogViewModel(shell.CurrentSolution);
                shell.ModalDialog.ShowDialog();
            });
        }

        private ReactiveCommand<object> command;

        public override ICommand Command
        {
            get
            {
                return command;
            }
        }

        public override string Name
        {
            get
            {
                return "File.NewSolution";
            }
        }

        public override string Text
        {
            get
            {
                return "New Solution";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Creates a new Solution";
            }
        }
    }
}
