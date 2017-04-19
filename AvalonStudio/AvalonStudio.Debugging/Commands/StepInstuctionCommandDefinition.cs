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

        private readonly ReactiveCommand<object> command;

        public StepInstructionCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager2>();

                manager.StepInstruction();
            });
        }

        public override Path IconPath
        {
            get
            {
                return new Path
                {
                    Fill = Brush.Parse("#FF8DD28A"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data = StreamGeometry.Parse("M8,5.14V19.14L19,12.14L8,5.14Z")
                };
            }
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