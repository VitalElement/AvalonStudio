using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Commands
{
	public interface ICommandHandler<TCommandDefinition> : ICommandHandler
		where TCommandDefinition : CommandDefinition
	{
		void Update(Command command);
		Task Run(Command command);
	}

	public interface ICommandListHandler<TCommandDefinition> : ICommandHandler
		where TCommandDefinition : CommandListDefinition
	{
		void Populate(Command command, List<Command> commands);
		Task Run(Command command);
	}

	public interface ICommandHandler
	{
	}

	public interface ICommandListHandler : ICommandHandler
	{
	}

	public abstract class CommandHandlerBase<TCommandDefinition> : ICommandHandler<TCommandDefinition>
		where TCommandDefinition : CommandDefinition
	{
		public virtual void Update(Command command)
		{
		}

		public abstract Task Run(Command command);
	}
}