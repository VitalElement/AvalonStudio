using AvalonStudio.Commands;
using AvalonStudio.Controls;
using AvalonStudio.Controls.Standard.ExtensionManagerDialog;
using AvalonStudio.Extensibility;
using System.Composition;
using ReactiveUI;
using AvalonStudio.Controls.Standard.SettingsDialog;

namespace AvalonStudio.Shell.Commands
{
    internal class ToolsCommands
    {
        [ExportCommandDefinition("Tools.Packages")]
        public CommandDefinition PackagesCommand { get; }

        [ExportCommandDefinition("Tools.Extensions")]
        public CommandDefinition ExtensionsCommand { get; }

        [ExportCommandDefinition("Tools.Options")]
        public CommandDefinition OptionsCommand { get; }

        private readonly IShell _shell;
        private readonly IExtensionManager _extensionManager;

        [ImportingConstructor]
        public ToolsCommands(IExtensionManager extensionManager)
        {
            _shell = IoC.Get<IShell>();

            _extensionManager = extensionManager;

            PackagesCommand = new CommandDefinition(
                "Packages", null, ReactiveCommand.Create(ShowPackagesDialog));

            ExtensionsCommand = new CommandDefinition(
                "Extensions", null, ReactiveCommand.Create(ShowExtensionsPage));

            OptionsCommand = new CommandDefinition(
                "Options", null, ReactiveCommand.Create(ShowOptionsPage));
        }

        private void ShowPackagesDialog()
        {
            _shell.ModalDialog = new PackageManagerDialogViewModel();
            _shell.ModalDialog.ShowDialogAsync();
        }

        private void ShowExtensionsPage() =>
            _shell.AddDocument(new ExtensionManagerDialogViewModel(_extensionManager));

        private void ShowOptionsPage() => _shell.AddOrSelectDocument(IoC.Get<SettingsDialogViewModel>());
    }
}
