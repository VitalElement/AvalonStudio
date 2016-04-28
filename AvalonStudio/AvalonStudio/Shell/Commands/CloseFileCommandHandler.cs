namespace AvalonStudio.Shell.Commands
{
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using AvalonStudio.Extensibility.Commands;
    using System;

    [CommandHandler]
    public class CloseFileCommandHandler : CommandHandlerBase<CloseFileCommandDefinition>
    {
        public CloseFileCommandHandler()
        {
            
        }

        public override void Update(Command command)
        {
            //command.Enabled = _shell.ActiveItem != null;
            //base.Update(command);
        }

        public override Task Run(Command command)
        {
            throw new NotImplementedException();
            //_shell.CloseDocument(_shell.ActiveItem);
            //return TaskUtility.Completed;
        }
    }
}