using AvalonStudio.Commands;
using AvalonStudio.Extensibility;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Composition;
using System.Reactive.Linq;

namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
    internal class SolutionExplorerCommands
    {
        [ExportCommandDefinition("File.NewSolution")]
        [DefaultKeyGestures("CTRL+SHIFT+N")]
        public CommandDefinition NewSolutionCommand { get; }

        [ExportCommandDefinition("File.OpenSolution")]
        [DefaultKeyGestures("CTRL+SHIFT+O")]
        public CommandDefinition OpenSolutionCommand { get; }

        [ExportCommandDefinition("File.CloseSolution")]
        public CommandDefinition CloseSolutionCommand { get; }

        private readonly IShell _shell;
        private readonly ISolutionExplorer _solutionExplorer;

        [ImportingConstructor]
        public SolutionExplorerCommands(ISolutionExplorer solutionExplorer)
        {
            _shell = IoC.Get<IShell>();
            _solutionExplorer = solutionExplorer;

            NewSolutionCommand = new CommandDefinition(
                "New Solution", null, ReactiveCommand.Create(_solutionExplorer.NewSolution));

            OpenSolutionCommand = new CommandDefinition(
                "Open Solution", null, ReactiveCommand.Create(_solutionExplorer.OpenSolution));

            CloseSolutionCommand = new CommandDefinition(
                "Close Solution", null, ReactiveCommand.Create(
                    _shell.CloseSolutionAsync, _shell.OnSolutionChanged.Select(s => s != null)));
        }
    }
}
