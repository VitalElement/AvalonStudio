using System.Windows.Input;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;

namespace AvalonStudio.Shell.Commands
{
	[CommandDefinition]
	public class CloseFileCommandDefinition : CommandDefinition
	{
		private ReactiveCommand<object> _command;
		public override string Text => "Close";

		public override string ToolTip => "Close ToolTip";
		public override ICommand Command => _command;
	}
}