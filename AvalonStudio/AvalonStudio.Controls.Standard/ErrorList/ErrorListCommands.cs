using AvalonStudio.Commands;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using ReactiveUI;
using System.Composition;
using System.Reactive.Linq;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    internal class ErrorListCommands
    {
        [ExportCommandDefinition("View.ErrorList")]
        public CommandDefinition ViewErrorListCommand { get; }

        private readonly IShell _shell;
        private readonly IErrorList _errorList;

        [ImportingConstructor]
        public ErrorListCommands(IErrorList solutionExplorer)
        {
            _shell = IoC.Get<IShell>();
            _errorList = solutionExplorer;

            ViewErrorListCommand = new CommandDefinition("Error List", null,
                ReactiveCommand.Create(() =>
                {
                    _shell.CurrentPerspective.AddOrSelectTool(_errorList as IToolViewModel);
                }));
        }
    }
}
