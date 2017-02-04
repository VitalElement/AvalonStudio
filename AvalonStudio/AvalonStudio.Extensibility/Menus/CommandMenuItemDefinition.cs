using System;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;

namespace AvalonStudio.Extensibility.Menus
{
	public class CommandMenuItemDefinition<TCommandDefinition> : MenuItemDefinition
		where TCommandDefinition : CommandDefinitionBase
	{
		private readonly CommandDefinitionBase _commandDefinition;

		public CommandMenuItemDefinition(Func<MenuItemGroupDefinition> group, int sortOrder)
			: base(group, sortOrder)
		{
			var commandService = IoC.Get<ICommandService>();
			_commandDefinition = commandService.GetCommandDefinition(typeof (TCommandDefinition));
			KeyGesture = IoC.Get<ICommandKeyGestureService>().GetPrimaryKeyGesture(_commandDefinition);
		}

		public override string Text
		{
			get { return _commandDefinition.Text; }
		}

		public override Uri IconSource
		{
			get { return _commandDefinition.IconSource; }
		}

		public override KeyGesture KeyGesture { get; }

		public override CommandDefinitionBase CommandDefinition
		{
			get { return _commandDefinition; }
		}
	}
}