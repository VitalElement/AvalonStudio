namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using System;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;

    [CommandHandler]
    public class ExitCommandHandler : CommandHandlerBase<ExitCommandDefinition>
    {
        public ExitCommandHandler()
        {
            
        }

        public override Task Run(Command command)
        {
            throw new NotImplementedException();
            //_shell.Close();
            //return TaskUtility.Completed;
        }
    }
}