using System.ComponentModel.Composition;
using System.Windows.Input;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using Key = Avalonia.Input.Key;

namespace AvalonStudio.Shell.Commands
{
	[CommandDefinition]
	public class NewFileCommandDefinition : CommandDefinition
	{
		[Export] public static CommandKeyboardShortcut KeyGesture =
			new CommandKeyboardShortcut<NewFileCommandDefinition>(new KeyGesture
			{
				Key = Key.N,
				Modifiers = InputModifiers.Control
			});

		private ReactiveCommand<object> _command;
		public override string Text => "New File";

		public override string ToolTip => "Creates a new file.";
		public override ICommand Command => _command;
	}
}