using System.ComponentModel.Composition;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;

namespace AvalonStudio.Extensibility.Commands
{
	[Export(typeof (ICommandKeyGestureService))]
	public class CommandKeyGestureService : ICommandKeyGestureService
	{
		private readonly ICommandService _commandService;
		private readonly CommandKeyboardShortcut[] _keyboardShortcuts;

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
					(uiElement as Window)?.KeyBindings.Add(new KeyBinding
					{
						Gesture = keyboardShortcut.KeyGesture,
						Command = keyboardShortcut.CommandDefinition.Command
					});
				}
			}
		}

		public KeyGesture GetPrimaryKeyGesture(CommandDefinitionBase commandDefinition)
		{
			var keyboardShortcut = _keyboardShortcuts.FirstOrDefault(x => x.CommandDefinition == commandDefinition);
			return keyboardShortcut != null
				? keyboardShortcut.KeyGesture
				: null;
		}
	}
}