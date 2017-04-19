using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Debugging.Commands
{
    internal class RestartDebuggingCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> command;

        public RestartDebuggingCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager2>();

                manager.Restart();
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
                    Data =
                        StreamGeometry.Parse(
                            "F1 M 0,4.30496L 5.03999,0L 5.03999,2.625L 18,2.625C 18.665,2.625 19.2312,2.85498 19.6987,3.31497C 20.1662,3.77499 20.4,4.33249 20.4,4.98746L 20.4,11.805C 20.4,12.46 20.1687,13.0138 19.7062,13.4662C 19.2438,13.9187 18.68,14.145 18.015,14.145L 1.67999,14.145L 1.67999,10.785L 17.04,10.785L 17.04,5.98499L 5.03999,5.98499L 5.03999,8.48251L 0,4.30496 Z ")
                };
            }
        }

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Restart Debugging"; }
        }

        public override string ToolTip
        {
            get { return "Restarts a debug session."; }
        }
    }
}