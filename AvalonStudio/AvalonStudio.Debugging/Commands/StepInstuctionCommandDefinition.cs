using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Debugging.Commands
{
    internal class StepInstructionCommandDefinition : CommandDefinition
    {
        public override KeyGesture Gesture => KeyGesture.Parse("F9");

        private ReactiveCommand command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(manager.StepInstruction, manager.CanStep);
        }
        
        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Step Instruction"; }
        }

        public override string ToolTip
        {
            get { return "Steps a single instruction."; }
        }
    }
}