using System;

namespace AvalonStudio.Extensibility.Commands
{
	public interface ICommandService
	{
		CommandDefinitionBase GetCommandDefinition(Type commandDefinitionType);
		Command GetCommand(CommandDefinitionBase commandDefinition);
	}
}