using System;
using System.Threading.Tasks;
using AvalonStudio.Extensibility.Commands;

namespace AvalonStudio.Shell.Commands
{
	[CommandHandler]
	public class ExitCommandHandler : CommandHandlerBase<ExitCommandDefinition>
	{
		public override Task Run(Command command)
		{
			throw new NotImplementedException();
			//_shell.Close();
			//return TaskUtility.Completed;
		}
	}
}