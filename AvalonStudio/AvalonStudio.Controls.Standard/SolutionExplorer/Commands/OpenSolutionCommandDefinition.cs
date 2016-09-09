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
using System.Linq;
using System.IO;

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


                List<string> allExtensions = new List<string>();

                foreach (var solutionType in shell.SolutionTypes)
                {
                    allExtensions.AddRange(solutionType.Extensions);
                }

                dlg.Filters.Add(new FileDialogFilter
                {
                    Name = "All Supported Solution Types",
                    Extensions = allExtensions
                });

                foreach (var solutionType in shell.SolutionTypes)
                {
                    dlg.Filters.Add(new FileDialogFilter
                    {
                        Name = solutionType.Description,
                        Extensions = solutionType.Extensions
                    });
                }
				
				dlg.InitialFileName = string.Empty;
				dlg.InitialDirectory = Platform.ProjectDirectory;
				var result = await dlg.ShowAsync();

				if (result != null)
				{
                    await shell.OpenSolution(result[0]);                    
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