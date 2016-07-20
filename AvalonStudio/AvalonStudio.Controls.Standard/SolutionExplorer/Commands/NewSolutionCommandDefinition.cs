using System;
using System.Windows.Input;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
	[CommandDefinition]
	internal class NewSolutionCommandDefinition : CommandDefinition
	{
		private readonly ReactiveCommand<object> command;

		public NewSolutionCommandDefinition()
		{
			command = ReactiveCommand.Create();
			command.Subscribe(_ =>
			{
				var shell = IoC.Get<IShell>();
				shell.ModalDialog = new NewProjectDialogViewModel(shell.CurrentSolution);
				shell.ModalDialog.ShowDialog();
			});
		}

		public override ICommand Command
		{
			get { return command; }
		}

		public override string Text
		{
			get { return "New Solution"; }
		}

		public override string ToolTip
		{
			get { return "Creates a new Solution"; }
		}
	}
}