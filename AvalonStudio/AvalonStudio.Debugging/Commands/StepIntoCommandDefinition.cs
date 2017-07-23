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

        private readonly ReactiveCommand<object> command;

        public StepIntoCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager2>();

                manager.StepInto();
            });
        }

        public override Avalonia.Controls.Shapes.Path IconPath
        {
            get
            {
                return new Avalonia.Controls.Shapes.Path
                {
                    Fill = Brush.Parse("#FF7AC1FF"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data =
                        StreamGeometry.Parse(
                            "M12,22A2,2 0 0,1 10,20A2,2 0 0,1 12,18A2,2 0 0,1 14,20A2,2 0 0,1 12,22M13,2V13L17.5,8.5L18.92,9.92L12,16.84L5.08,9.92L6.5,8.5L11,13V2H13Z")
                };
            }
        }

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