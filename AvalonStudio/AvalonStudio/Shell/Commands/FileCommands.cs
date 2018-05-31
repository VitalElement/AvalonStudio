using AvalonStudio.Commands;
using System;
using System.Composition;
using ReactiveUI;
using AvalonStudio.Extensibility;

namespace AvalonStudio.Shell.Commands
{
    internal class FileCommands
    {
        [ExportCommandDefinition("File.Save")]
        [DefaultKeyGestures("CTRL+S")]
        public CommandDefinition SaveCommand { get; }

        [ExportCommandDefinition("File.SaveAll")]
        public CommandDefinition SaveAllCommand { get; }

        [ExportCommandDefinition("File.Exit")]
        [DefaultKeyGestures("ALT+F4")]
        public CommandDefinition ExitCommand { get; }

        private readonly IShell _shell;

        [ImportingConstructor]
        public FileCommands(CommandIconService commandIconService)
        {
            _shell = IoC.Get<IShell>();

            SaveCommand = new CommandDefinition(
                "Save",
                commandIconService.GetCompletionKindImage("Save"),
                ReactiveCommand.Create(Save));

            SaveAllCommand = new CommandDefinition(
                "Save All",
                commandIconService.GetCompletionKindImage("SaveAll"),
                ReactiveCommand.Create(SaveAll));

            ExitCommand = new CommandDefinition("Exit", null, ReactiveCommand.Create(Exit));
        }

        private void Save() => _shell.Save();
        private void SaveAll() => _shell.SaveAll();

        private void Exit() => Environment.Exit(0);
    }
}
