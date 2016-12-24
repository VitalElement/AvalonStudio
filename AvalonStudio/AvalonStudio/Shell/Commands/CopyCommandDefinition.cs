using System.Windows.Input;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;

namespace AvalonStudio.Shell.Commands
{
	[CommandDefinition]
	public class CopyCommandDefinition : CommandDefinition
	{
		private ReactiveCommand<object> _command;
		public override string Text => "Copy";

		public override string ToolTip => "Copy ToolTip";
		public override ICommand Command => _command;
	}
}