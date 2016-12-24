namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    [CommandHandler]
    public class SwitchToDocumentListCommandHandler : ICommandListHandler<SwitchToDocumentCommandListDefinition>
    {
        private readonly IShell _shell;

        [ImportingConstructor]
        public SwitchToDocumentListCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public void Populate(Command command, List<Command> commands)
        {
            for (var i = 0; i < _shell.Documents.Count; i++)
            {
                var document = _shell.Documents[i];
                commands.Add(new Command(command.CommandDefinition)
                {
                    Checked = _shell.ActiveItem == document,
                    Text = string.Format("_{0} {1}", i + 1, document.DisplayName),
                    Tag = document
                });
            }
        }

        public Task Run(Command command)
        {
            _shell.OpenDocument((IDocument) command.Tag);
            return TaskUtility.Completed;
        }
    }
}