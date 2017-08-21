namespace AvalonStudio.Debugging.Commands
{
    using Avalonia.Input;
    using Avalonia.Media;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    using System;
    using System.Reactive.Linq;
    using System.Windows.Input;

    internal class StartDebuggingCommandDefinition : CommandDefinition
    {
        private ReactiveCommand<object> command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(manager.CanStart);
            command.Subscribe(_ =>
            {
                if (!manager.SessionActive)
                {
                    manager.Start();
                }
                else
                {
                    manager.Continue();
                }
            });
        }

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Start Debugging"; }
        }

        public override string ToolTip
        {
            get { return "Starts a debug session."; }
        }

       /* public override Avalonia.Controls.Shapes.Path Icon
        {
            get
            {
                return new Avalonia.Controls.Shapes.Path
                {
                    Fill = Brush.Parse("#FF8DD28A"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data = StreamGeometry.Parse("M8,5.14V19.14L19,12.14L8,5.14Z")
                };
            }
        }*/

        public override KeyGesture Gesture => KeyGesture.Parse("F5");
    }
}