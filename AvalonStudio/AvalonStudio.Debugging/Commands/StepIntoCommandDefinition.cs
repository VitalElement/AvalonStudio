namespace AvalonStudio.Debugging.Commands
{
    using Avalonia.Input;
    using Avalonia.Media;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    using System;
    using System.Windows.Input;

    internal class StepIntoCommandDefinition : CommandDefinition
    {
        public override KeyGesture Gesture => KeyGesture.Parse("F11");

        private ReactiveCommand command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(()=>manager.StepInto(), manager.CanStep);
        }

        public override DrawingGroup Icon => this.GetCommandIcon("StepInto");

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Step Into"; }
        }

        public override string ToolTip
        {
            get { return "Steps into the current line."; }
        }
    }
}