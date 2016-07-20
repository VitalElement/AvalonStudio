using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using Key = Avalonia.Input.Key;

namespace AvalonStudio.Shell.Commands
{
	[CommandDefinition]
	public class ExitCommandDefinition : CommandDefinition
	{
		[Export] public static CommandKeyboardShortcut KeyGesture =
			new CommandKeyboardShortcut<ExitCommandDefinition>(new KeyGesture {Key = Key.F4, Modifiers = InputModifiers.Alt});

		private readonly ReactiveCommand<object> _command;

		public ExitCommandDefinition()
		{
			_command = ReactiveCommand.Create();

			_command.Subscribe(_ => { Environment.Exit(0); });
		}

		public override string Text => "Exit";

		public override string ToolTip => "Exit Tool Tip";
		public override ICommand Command => _command;
	}
}