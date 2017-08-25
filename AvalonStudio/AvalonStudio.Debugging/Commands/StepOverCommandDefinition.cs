namespace AvalonStudio.Debugging.Commands
{
    using Avalonia.Input;
    using Avalonia.Media;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    using System;
    using System.Windows.Input;

    internal class StepOverCommandDefinition : CommandDefinition
    {
        public override KeyGesture Gesture => KeyGesture.Parse("F10");

        private ReactiveCommand<object> command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(manager.CanStep);

            command.Subscribe(_ =>
            {
                manager.StepOver();
            });
        }

        public override DrawingGroup Icon => this.GetCommandIcon("StepOver");

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Step Over"; }
        }

        public override string ToolTip
        {
            get { return "Steps over the current line."; }
        }
    }
}