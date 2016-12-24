using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace AvalonStudio.Extensibility.Commands
{
	[Export(typeof (ICommandService))]
	public class CommandService : ICommandService
	{
		private readonly Dictionary<Type, CommandDefinitionBase> _commandDefinitionsLookup;
		private readonly Dictionary<CommandDefinitionBase, Command> _commands;

#pragma warning disable 649
		[ImportMany] private CommandDefinitionBase[] _commandDefinitions;
#pragma warning restore 649

		public CommandService()
		{
			_commandDefinitionsLookup = new Dictionary<Type, CommandDefinitionBase>();
			_commands = new Dictionary<CommandDefinitionBase, Command>();
		}

		public CommandDefinitionBase GetCommandDefinition(Type commandDefinitionType)
		{
			CommandDefinitionBase commandDefinition;
			if (!_commandDefinitionsLookup.TryGetValue(commandDefinitionType, out commandDefinition))
				commandDefinition = _commandDefinitionsLookup[commandDefinitionType] =
					_commandDefinitions.First(x => x.GetType() == commandDefinitionType);
			return commandDefinition;
		}

		public Command GetCommand(CommandDefinitionBase commandDefinition)
		{
			Command command;
			if (!_commands.TryGetValue(commandDefinition, out command))
				command = _commands[commandDefinition] = new Command(commandDefinition);
			return command;
		}
	}
}