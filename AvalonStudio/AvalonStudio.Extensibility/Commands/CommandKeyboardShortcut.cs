using System;
using Avalonia.Input;
using System.ComponentModel.Composition;

namespace AvalonStudio.Extensibility.Commands
{
    [InheritedExport]
	public abstract class CommandKeyboardShortcut
	{
		private readonly Func<CommandDefinitionBase> _commandDefinition;

		protected CommandKeyboardShortcut(KeyGesture keyGesture, int sortOrder, Func<CommandDefinitionBase> commandDefinition)
		{
			_commandDefinition = commandDefinition;
			KeyGesture = keyGesture;
			SortOrder = sortOrder;
		}

		public CommandDefinitionBase CommandDefinition
		{
			get { return _commandDefinition(); }
		}

		public KeyGesture KeyGesture { get; }

		public int SortOrder { get; }
	}

	public class CommandKeyboardShortcut<TCommandDefinition> : CommandKeyboardShortcut
		where TCommandDefinition : CommandDefinition
	{
		public CommandKeyboardShortcut(KeyGesture keyGesture, int sortOrder = 5)
			: base(keyGesture, sortOrder, () => IoC.Get<ICommandService>().GetCommandDefinition(typeof (TCommandDefinition)))
		{
		}
	}
}