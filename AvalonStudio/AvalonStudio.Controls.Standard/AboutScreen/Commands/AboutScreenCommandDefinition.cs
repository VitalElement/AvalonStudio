using Avalonia.Input;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Controls.Standard.AboutScreen.Commands
{
    internal class AboutScreenCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand command;

        public AboutScreenCommandDefinition()
        {
            command = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();
                shell.ModalDialog = new AboutDialogViewModel();
                shell.ModalDialog.ShowDialog();
            });
        }

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "About"; }
        }

        public override string ToolTip
        {
            get { return "Opens the About Screen"; }
        }

        public override KeyGesture Gesture => null;
    }
}