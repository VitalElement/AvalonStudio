using AvalonStudio.Extensibility;
using AvalonStudio.Commands;
using ReactiveUI;
using AvalonStudio.Shell;

namespace AvalonStudio.Controls.Standard.FindInFiles.Commands
{
    internal class FindInFilesCommands
    {
        [ExportCommandDefinition("Edit.FindInFiles")]
        [DefaultKeyGestures("CTRL + SHIFT + F")]
        public CommandDefinition FindInFilesCommandDefinition { get; }

        public FindInFilesCommands()
        {
            FindInFilesCommandDefinition = new CommandDefinition(
                "Find in Files",
                null,
                ReactiveCommand.Create(() =>
                {
                    var vm = IoC.Get<FindInFilesViewModel>();

                    IoC.Get<IShell>().CurrentPerspective.AddOrSelectTool(vm);
                }));
        }
    }
}
