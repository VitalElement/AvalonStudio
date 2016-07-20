namespace AvalonStudio.Shell.Commands
{
	using System;
	using System.ComponentModel.Composition;
	using Avalonia.Input;
	using Extensibility.Commands;
	using ReactiveUI;

	[CommandDefinition]
	public class ExitCommandDefinition : CommandDefinition
	{
		public ExitCommandDefinition()
		{
			_command = ReactiveCommand.Create();

			_command.Subscribe(_ =>
			{
				Environment.Exit(0);
			});
		}

		public override string Text => "Exit";

		public override string ToolTip => "Exit Tool Tip";

		readonly ReactiveCommand<object> _command;
		public override System.Windows.Input.ICommand Command => _command;

		[Export]
		public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<ExitCommandDefinition>(new KeyGesture() { Key = Key.F4, Modifiers = InputModifiers.Alt });
	}
}