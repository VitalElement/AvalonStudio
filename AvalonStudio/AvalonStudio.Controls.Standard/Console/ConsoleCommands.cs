using AvalonStudio.Commands;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using ReactiveUI;
using System.Composition;
using System.Reactive.Linq;

namespace AvalonStudio.Controls.Standard.Console
{
    internal class ConsoleCommands
    {
        [ExportCommandDefinition("View.Console")]
        public CommandDefinition ViewConsoleCommand { get; }

        private readonly IShell _shell;
        private readonly IConsole _console;

        [ImportingConstructor]
        public ConsoleCommands(IConsole solutionExplorer)
        {
            _shell = IoC.Get<IShell>();
            _console = solutionExplorer;

            ViewConsoleCommand = new CommandDefinition("Console", null,
                ReactiveCommand.Create(() =>
                {
                    _shell.AddOrSelectTool(_console as IToolViewModel);
                }));
        }
    }
}
