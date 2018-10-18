using AvalonStudio.Commands;
using System;
using System.Composition;
using ReactiveUI;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using System.Reactive.Linq;
using Avalonia;

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

        private readonly IStudio _studio;
        private readonly IShell _shell;

        [ImportingConstructor]
        public FileCommands(CommandIconService commandIconService)
        {
            _studio = IoC.Get<IStudio>();
            _shell = IoC.Get<IShell>();

            SaveCommand = new CommandDefinition(
                "Save",
                commandIconService.GetCompletionKindImage("Save"),
                ReactiveCommand.Create(Save, _shell.WhenAnyValue(x=>x.SelectedDocument).Select(doc => doc != null)));

            SaveAllCommand = new CommandDefinition(
                "Save All",
                commandIconService.GetCompletionKindImage("SaveAll"),
                ReactiveCommand.Create(SaveAll));

            ExitCommand = new CommandDefinition("Exit", null, ReactiveCommand.Create(Exit));
        }

        private void Save() => _studio.Save();
        private void SaveAll() => _studio.SaveAll();

        private void Exit() => Application.Current.Exit();
    }
}
