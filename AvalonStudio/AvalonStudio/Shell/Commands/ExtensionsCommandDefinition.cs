using AvalonStudio.Controls.Standard.ExtensionManagerDialog;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class ExtensionsCommandDefinition : CommandDefinition
    {
        private ReactiveCommand _command;

        public ExtensionsCommandDefinition()
        {
            _command = ReactiveCommand.Create(
                () =>
                {
                    var shell = IoC.Get<IShell>();
                    shell.AddDocument(new ExtensionManagerDialogViewModel());
                });
        }

        public override string Text => "Extensions";
        public override string ToolTip => "Extensions";
        public override ICommand Command => _command;
    }
}
