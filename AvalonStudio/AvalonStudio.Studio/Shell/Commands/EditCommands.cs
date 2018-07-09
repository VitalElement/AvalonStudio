using AvalonStudio.Commands;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using ReactiveUI;
using System.Composition;

namespace AvalonStudio.Studio.Shell.Commands
{
    internal class EditCommands
    {
        [ExportCommandDefinition("Edit.ShowQuickCommander")]
        [DefaultKeyGestures("CTRL+P")]
        public CommandDefinition ShowQuickCommanderCommand { get; }

        [ImportingConstructor]
        public EditCommands(CommandIconService commandIconService)
        {
            ShowQuickCommanderCommand = new CommandDefinition(
              "Show Quick Commander", null, ReactiveCommand.Create(ShowQuickCommander));
        }

        private void ShowQuickCommander() => IoC.Get<IStudio>().ShowQuickCommander();
    }
}
