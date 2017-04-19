namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    using System;
    using System.Windows.Input;

    internal class OpenSolutionCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> command;

        public OpenSolutionCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                IoC.Get<ISolutionExplorer>().OpenSolution();
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