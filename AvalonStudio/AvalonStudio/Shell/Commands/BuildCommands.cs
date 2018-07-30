using AvalonStudio.Commands;
using AvalonStudio.Controls.Standard.SolutionExplorer;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
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

        private readonly IStudio _studio;
        private readonly ISolutionExplorer _solutionExplorer;

        [ImportingConstructor]
        public BuildCommands(
            ISolutionExplorer solutionExplorer,
            CommandIconService commandIconService)
        {
            _studio = IoC.Get<IStudio>();
            _solutionExplorer = solutionExplorer;

            var shellCanRunTask = _studio.CanRunTask();

            BuildCommand = new CommandDefinition(
                "Build",
                commandIconService.GetCompletionKindImage("Build"),
                ReactiveCommand.Create(_studio.Build, shellCanRunTask));

            CleanCommand = new CommandDefinition(
                "Clean",
                commandIconService.GetCompletionKindImage("Clean"),
                ReactiveCommand.Create(_studio.Clean, shellCanRunTask));
        }
    }
}
