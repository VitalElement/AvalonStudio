using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class BuildCommandDefinition : CommandDefinition
    {
        private ReactiveCommand<object> _command;

        public override void Activation()
        {
            var shell = IoC.Get<IShell>();

            _command = ReactiveCommand.Create(shell.CanRunTask());

            _command.Subscribe(_ =>
            {
                shell.Build();
            });
        }

        public override string Text => "Build";

        public override string ToolTip => "Builds the Startup Project";

        public override Path IconPath
            =>
                new Path
                {
                    Fill = Brush.Parse("#FFDAA107"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data =
                        StreamGeometry.Parse(
                            "M21,16.5C21,16.88 20.79,17.21 20.47,17.38L12.57,21.82C12.41,21.94 12.21,22 12,22C11.79,22 11.59,21.94 11.43,21.82L3.53,17.38C3.21,17.21 3,16.88 3,16.5V7.5C3,7.12 3.21,6.79 3.53,6.62L11.43,2.18C11.59,2.06 11.79,2 12,2C12.21,2 12.41,2.06 12.57,2.18L20.47,6.62C20.79,6.79 21,7.12 21,7.5V16.5M12,4.15L6.04,7.5L12,10.85L17.96,7.5L12,4.15Z")
                };

        public override ICommand Command => _command;

        public override KeyGesture Gesture => KeyGesture.Parse("F6");
    }
}