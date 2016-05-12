namespace AvalonStudio.Controls.Standard.AboutScreen.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using Extensibility;
    using ReactiveUI;
    using Shell;
    using System;
    using System.Windows.Input;

    [CommandDefinition]
    class AboutScreenCommandDefinition : CommandDefinition
    {
        public AboutScreenCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                IShell shell = IoC.Get<IShell>();
                shell.ModalDialog = new AboutDialogViewModel();
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

        public override string Text
        {
            get
            {
                return "About";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Opens the About Screen";
            }
        }
    }
}
