using System;
using System.Threading.Tasks;
using AvalonStudio.Extensibility.Commands;

namespace AvalonStudio.Shell.Commands
{
	[CommandHandler]
	public class CloseFileCommandHandler : CommandHandlerBase<CloseFileCommandDefinition>
	{
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