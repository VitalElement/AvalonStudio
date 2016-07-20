using System.ComponentModel.Composition;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;

namespace AvalonStudio.Shell.Commands
{
	[CommandDefinition]
	public class NewFileCommandDefinition : CommandDefinition
	{
		public override string Text => "New File";

		public override string ToolTip => "Creates a new file.";

		ReactiveCommand<object> _command;
		public override System.Windows.Input.ICommand Command => _command;

		[Export]
		public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<NewFileCommandDefinition>(new KeyGesture() { Key = Key.N, Modifiers = InputModifiers.Control });
	}
}