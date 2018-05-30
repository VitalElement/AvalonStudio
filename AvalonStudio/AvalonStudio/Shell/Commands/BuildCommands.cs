using AvalonStudio.Commands;
using AvalonStudio.Controls.Standard.SolutionExplorer;
using AvalonStudio.Extensibility;
using ReactiveUI;
using System.Composition;

namespace AvalonStudio.Shell.Commands
{
    internal class BuildCommands
    {
        [ExportCommandDefinition("Build.Build")]
        [DefaultKeyGestures("F6")]
        public CommandDefinition BuildCommand { get; }

        [ExportCommandDefinition("Build.Clean")]
        public CommandDefinition CleanCommand { get; }

        private readonly IShell _shell;
        private readonly ISolutionExplorer _solutionExplorer;

        [ImportingConstructor]
        public BuildCommands(
            ISolutionExplorer solutionExplorer,
            CommandIconService commandIconService)
        {
            _shell = IoC.Get<IShell>();
            _solutionExplorer = solutionExplorer;

            var shellCanRunTask = _shell.CanRunTask();

            BuildCommand = new CommandDefinition(
                "Build",
                commandIconService.GetCompletionKindImage("Build"),
                ReactiveCommand.Create(_shell.Build, shellCanRunTask));

            CleanCommand = new CommandDefinition(
                "Clean",
                commandIconService.GetCompletionKindImage("Clean"),
                ReactiveCommand.Create(_shell.Clean, shellCanRunTask));
        }
    }
}
