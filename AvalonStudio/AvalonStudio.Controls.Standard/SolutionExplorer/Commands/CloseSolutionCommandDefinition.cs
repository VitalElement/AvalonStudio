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
    class CloseSolutionCommandDefinition : CommandDefinition
    {
        public CloseSolutionCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe( _ =>
            {
                IShell shell = IoC.Get<IShell>();
                shell.CurrentSolution = null;
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

        public override string Name
        {
            get
            {
                return "File.CloseSolution";
            }
        }

        public override string Text
        {
            get
            {
                return "Close Solution";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Closes the current Solution";
            }
        }
    }
}
