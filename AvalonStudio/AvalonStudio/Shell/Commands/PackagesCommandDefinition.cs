using AvalonStudio.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class PackagesCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> _command;

        public PackagesCommandDefinition()
        {
            _command = ReactiveCommand.Create();

            _command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();
                shell.ModalDialog = new PackageManagerDialogViewModel();
                shell.ModalDialog.ShowDialog();
            });
        }

        public override string Text => "Packages";

        public override string ToolTip => "Packages Tool Tip";
        public override ICommand Command => _command;
    }
}