using AvalonStudio.Controls.Standard.ExtensionManagerDialog;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System.Composition;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class ExtensionsCommandDefinition : CommandDefinition
    {
        private ReactiveCommand _command;

        [ImportingConstructor]
        public ExtensionsCommandDefinition(IExtensionManager extensionManager)
        {
            _command = ReactiveCommand.Create(
                () =>
                {
                    var shell = IoC.Get<IShell>();
                    shell.AddDocument(new ExtensionManagerDialogViewModel(extensionManager));
                });
        }

        public override string Text => "Extensions";
        public override string ToolTip => "Extensions";
        public override ICommand Command => _command;
    }
}
