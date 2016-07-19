namespace AvalonStudio.Shell.Commands
{
	using AvalonStudio.Extensibility.Commands;
	using ReactiveUI;
	[CommandDefinition]
	public class CopyCommandDefinition : CommandDefinition
	{
		public override string Text => "Copy";

		public override string ToolTip => "Copy ToolTip";

		ReactiveCommand<object> _command;
		public override System.Windows.Input.ICommand Command => _command;
	}
}