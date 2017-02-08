using System;
using System.Windows.Input;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
	
	internal class CloseSolutionCommandDefinition : CommandDefinition
	{
		private readonly ReactiveCommand<object> command;

		public CloseSolutionCommandDefinition()
		{
			command = ReactiveCommand.Create();
			command.Subscribe(async _ =>
			{
				var shell = IoC.Get<IShell>();
                await shell.CloseSolutionAsync();
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