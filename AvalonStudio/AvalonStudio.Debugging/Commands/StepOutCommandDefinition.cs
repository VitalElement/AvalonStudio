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
    internal class StepOutCommandDefinition : CommandDefinition
    {
        private ReactiveCommand<object> command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(manager.CanStep);

            command.Subscribe(_ =>
            {
                manager.StepOut();
            });
        }

        public override KeyGesture Gesture => KeyGesture.Parse("SHIFT+F11");

        public override DrawingGroup Icon => this.GetCommandIcon("StepOut");

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Step Out"; }
        }

        public override string ToolTip
        {
            get { return "Steps out of the current function or method."; }
        }
    }
}