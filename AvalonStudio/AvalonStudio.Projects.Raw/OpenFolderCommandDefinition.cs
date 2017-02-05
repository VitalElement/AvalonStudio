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
using AvalonStudio.Projects.Raw;

namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
    
    public class OpenFolderCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> command;

        public OpenFolderCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(async _ =>
            {
                var shell = IoC.Get<IShell>();

                var ofd = new OpenFolderDialog();

                var result = await ofd.ShowAsync();

                if (result != string.Empty)
                {
                    //shell.CurrentSolution = RawProject.CreateRawSolution(result);
                }
            });
        }

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Open Folder"; }
        }

        public override string ToolTip
        {
            get { return "Opens a Folder"; }
        }
    }
}