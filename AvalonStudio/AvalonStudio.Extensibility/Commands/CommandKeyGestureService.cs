using Perspex.Controls;
using Perspex.Input;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace AvalonStudio.Extensibility.Commands
{
    [Export(typeof(ICommandKeyGestureService))]
    public class CommandKeyGestureService : ICommandKeyGestureService
    {
        private readonly CommandKeyboardShortcut[] _keyboardShortcuts;
        private readonly ICommandService _commandService;

        [ImportingConstructor]
        public CommandKeyGestureService(
            [ImportMany] CommandKeyboardShortcut[] keyboardShortcuts,
            [ImportMany] ExcludeCommandKeyboardShortcut[] excludeKeyboardShortcuts,
            ICommandService commandService)
        {
            _keyboardShortcuts = keyboardShortcuts
                .Except(excludeKeyboardShortcuts.Select(x => x.KeyboardShortcut))
                .OrderBy(x => x.SortOrder)
                .ToArray();
            _commandService = commandService;
        }

        public void BindKeyGestures(Control uiElement)
        {
            foreach (var keyboardShortcut in _keyboardShortcuts)
            {
                if (keyboardShortcut.KeyGesture != null)
                {
                    (uiElement as Window)?.KeyBindings.Add(new KeyBinding() { Gesture = keyboardShortcut.KeyGesture, Command = keyboardShortcut.CommandDefinition.Command });
                }
            }
        }

        public KeyGesture GetPrimaryKeyGesture(CommandDefinitionBase commandDefinition)
        {
            var keyboardShortcut = _keyboardShortcuts.FirstOrDefault(x => x.CommandDefinition == commandDefinition);
            return (keyboardShortcut != null)
                ? keyboardShortcut.KeyGesture
                : null;
        }
    }
}