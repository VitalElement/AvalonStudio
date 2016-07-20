using System;
using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
	[CommandDefinition]
	internal class OpenSolutionCommandDefinition : CommandDefinition
	{
		private readonly ReactiveCommand<object> command;

		public OpenSolutionCommandDefinition()
		{
			command = ReactiveCommand.Create();
			command.Subscribe(async _ =>
			{
				var shell = IoC.Get<IShell>();

				var dlg = new OpenFileDialog();
				dlg.Title = "Open Solution";
				dlg.Filters.Add(new FileDialogFilter
				{
					Name = "AvalonStudio Solution",
					Extensions = new List<string> {Solution.Extension}
				});
				dlg.InitialFileName = string.Empty;
				dlg.InitialDirectory = Platform.ProjectDirectory;
				var result = await dlg.ShowAsync();

				if (result != null)
				{
					shell.CurrentSolution = Solution.Load(result[0]);
				}
			});
		}

		public override ICommand Command
		{
			get { return command; }
		}

		public override string Text
		{
			get { return "Open Solution"; }
		}

		public override string ToolTip
		{
			get { return "Opens a Solution"; }
		}
	}
}