using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
    internal class CloseSolutionCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand command;

        public CloseSolutionCommandDefinition()
        {
            command = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();
                shell.CloseSolution();
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