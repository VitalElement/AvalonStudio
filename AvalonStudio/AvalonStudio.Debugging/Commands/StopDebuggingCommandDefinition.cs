using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Debugging.Commands
{
    internal class StopDebuggingCommandDefinition : CommandDefinition
    {
        private ReactiveCommand<object> command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(manager.CanStop);
            command.Subscribe(_ =>
            {
                manager.Stop();
            });
        }

        /*public override Path Icon
        {
            get
            {
                return new Path
                {
                    Fill = Brush.Parse("#FFF38B76"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data = StreamGeometry.Parse("M18,18H6V6H18V18Z")
                };
            }
        }*/

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Stop"; }
        }

        public override string ToolTip
        {
            get { return "Stops the current debug session."; }
        }
    }
}