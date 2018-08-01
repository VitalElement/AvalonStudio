using AvalonStudio.Commands;
using AvalonStudio.Controls.Standard.WelcomeScreen;
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

        [ExportCommandDefinition("View.WelcomeScreen")]
        public CommandDefinition ViewWelcomeScreenCommand { get; }

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
                    _shell.CurrentPerspective.AddOrSelectTool(_console as IToolViewModel);
                }));

            ViewWelcomeScreenCommand = new CommandDefinition("Welcome Screen", null,
              ReactiveCommand.Create(()=>
              {
                  var vm = IoC.Get<WelcomeScreenViewModel>();
                  _shell.AddOrSelectDocument(vm);
              }));
        }
    }
}
