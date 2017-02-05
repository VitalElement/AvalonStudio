namespace AvalonStudio.Controls.Standard.SolutionExplorer.Commands
{
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    using System;
    using System.Windows.Input;

    internal class NewSolutionCommandDefinition : CommandDefinition
    {
        public NewSolutionCommandDefinition()
        {
            var command = ReactiveCommand.Create();

            command.Subscribe(_ =>
            {
                IoC.Get<ISolutionExplorer>().NewSolution();
            });

            Command = command;            
        }

        public override ICommand Command { get; }

        public override string Text => "New Solution";

        public override string ToolTip => "Creates a new solution.";
    }
}