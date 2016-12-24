namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    using System;
    using System.Windows.Input;

    [CommandDefinition]
    internal class NewSolutionCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> command;

        public NewSolutionCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                IoC.Get<ISolutionExplorer>().NewSolution();
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