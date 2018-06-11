using AvalonStudio.Commands;
using AvalonStudio.Extensibility;
using AvalonStudio.Shell;
using ReactiveUI;

namespace ShellExampleApp.Commands
{
    internal class HelpCommands
    {
        [ExportCommandDefinition("Help.About")]
        public CommandDefinition AboutCommand { get; }

        private IShell _shell;

        public HelpCommands()
        {
            _shell = IoC.Get<IShell>();

            AboutCommand = new CommandDefinition(
                "About", null, ReactiveCommand.Create(ShowAboutDialog));
        }

        private void ShowAboutDialog()
        {
            _shell.ModalDialog = new AboutDialogViewModel();
            _shell.ModalDialog.ShowDialog();
        }
    }
}
