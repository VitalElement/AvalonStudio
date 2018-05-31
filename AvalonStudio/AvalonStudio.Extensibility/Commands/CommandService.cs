using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using AvalonStudio.Commands.Settings;

namespace AvalonStudio.Commands
{
    [Export]
    [Shared]
    public class CommandService
    {
        private readonly CommandSettingsService _commandSettingsService;
        private readonly IEnumerable<Lazy<CommandDefinition, CommandDefinitionMetadata>> _commands;

        private readonly Lazy<IImmutableDictionary<string, Lazy<CommandDefinition>>> _resolvedCommands;

        private IImmutableDictionary<CommandDefinition, IEnumerable<string>> _keyGestures;

        [ImportingConstructor]
        public CommandService(
            CommandSettingsService commandSettingsService,
            [ImportMany] IEnumerable<Lazy<CommandDefinition, CommandDefinitionMetadata>> commands)
        {
            _commandSettingsService = commandSettingsService;
            _commands = commands;

            _resolvedCommands = new Lazy<IImmutableDictionary<string, Lazy<CommandDefinition>>>(ResolveCommands);
        }

        public Lazy<CommandDefinition> GetCommand(string commandName)
        {
            var resolvedCommands = _resolvedCommands.Value;

            if (!resolvedCommands.TryGetValue(commandName, out var command))
            {
                // todo: log warning
            }

            return command;
        }

        public IImmutableDictionary<CommandDefinition, IEnumerable<string>> GetKeyGestures()
        {
            if (_keyGestures != null)
            {
                return _keyGestures;
            }

            var commandSettings = _commandSettingsService.GetCommandSettings();
            var builder = ImmutableDictionary.CreateBuilder<CommandDefinition, IEnumerable<string>>();

            foreach (var command in _commands)
            {
                if (!commandSettings.Commands.TryGetValue(command.Metadata.Name, out var settings))
                {
                    settings = new Command();

                    if (command.Metadata.DefaultKeyGestures != null)
                    {
                        settings.KeyGestures.AddRange(command.Metadata.DefaultKeyGestures);
                    }

                    commandSettings.Commands.Add(command.Metadata.Name, settings);
                }

                builder.Add(command.Value, settings.KeyGestures);
            }

            return _keyGestures = builder.ToImmutable();
        }

        private IImmutableDictionary<string, Lazy<CommandDefinition>> ResolveCommands()
        {
            var builder = ImmutableDictionary.CreateBuilder<string, Lazy<CommandDefinition>>();

            foreach (var command in _commands)
            {
                builder.Add(command.Metadata.Name, command);
            }

            return builder.ToImmutable();
        }
    }
}
