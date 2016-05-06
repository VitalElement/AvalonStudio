namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using Extensibility;
    using Perspex.Controls;
    using Projects;
    using ReactiveUI;
    using Shell;
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    [CommandDefinition]
    class OpenSolutionCommandDefinition : CommandDefinition
    {
        public OpenSolutionCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(async _ =>
            {
                IShell shell = IoC.Get<IShell>();

                var dlg = new OpenFileDialog();
                dlg.Title = "Open Solution";
                dlg.Filters.Add(new FileDialogFilter { Name = "AvalonStudio Solution", Extensions = new List<string> { Solution.Extension } });
                dlg.InitialFileName = string.Empty;
                dlg.InitialDirectory = Platforms.Platform.ProjectDirectory;
                var result = await dlg.ShowAsync();

                if (result != null)
                {
                    shell.CurrentSolution = Solution.Load(result[0]);
                }
            });
        }

        private ReactiveCommand<object> command;

        public override ICommand Command
        {
            get
            {
                return command;
            }
        }

        public override string Text
        {
            get
            {
                return "Open Solution";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Opens a Solution";
            }
        }
    }
}
