namespace AvalonStudio.Shell.Commands
{
	using AvalonStudio.Extensibility.Commands;
	using ReactiveUI;
	[CommandDefinition]
	public class CloseFileCommandDefinition : CommandDefinition
	{
		public override string Text => "Close";

		public override string ToolTip => "Close ToolTip";

		ReactiveCommand<object> _command;
		public override System.Windows.Input.ICommand Command => _command;
	}
}