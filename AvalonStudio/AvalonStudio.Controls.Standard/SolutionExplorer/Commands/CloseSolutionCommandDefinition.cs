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
            command = ReactiveCommand.Create(async () =>
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