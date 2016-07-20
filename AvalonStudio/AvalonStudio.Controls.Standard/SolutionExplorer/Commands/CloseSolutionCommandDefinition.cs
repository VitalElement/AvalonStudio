using System;
using System.Windows.Input;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
	[CommandDefinition]
	internal class CloseSolutionCommandDefinition : CommandDefinition
	{
		private readonly ReactiveCommand<object> command;

		public CloseSolutionCommandDefinition()
		{
			command = ReactiveCommand.Create();
			command.Subscribe(_ =>
			{
				var shell = IoC.Get<IShell>();
				shell.CurrentSolution = null;
			});
		}

		public override ICommand Command
		{
			get { return command; }
		}

		public override string Text
		{
			get { return "Close Solution"; }
		}

		public override string ToolTip
		{
			get { return "Closes the current Solution"; }
		}
	}
}