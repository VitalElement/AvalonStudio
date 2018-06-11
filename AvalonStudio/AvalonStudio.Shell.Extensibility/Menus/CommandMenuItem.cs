using Avalonia.Media;
using AvalonStudio.Commands;
using System;
using System.Windows.Input;

namespace AvalonStudio.Menus
{
    internal class CommandMenuItem : IMenuItem
    {
        public string Label => _commandDefinition.Value.Label;
        public DrawingGroup Icon => _commandDefinition.Value.Icon;

        public ICommand Command => _commandDefinition.Value.Command;

        private readonly CommandService _commandsService;
        private readonly string _commandName; 

        private readonly Lazy<CommandDefinition> _commandDefinition;

        public CommandMenuItem(CommandService commandsService, string commandName)
        {
            _commandsService = commandsService;
            _commandName = commandName;

            _commandDefinition = new Lazy<CommandDefinition>(ResolveCommandDefinition);
        }

        private CommandDefinition ResolveCommandDefinition() => _commandsService.GetCommand(_commandName).Value;
    }
}
