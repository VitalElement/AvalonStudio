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

        public override DrawingGroup Icon => this.GetCommandIcon("Build");


        public override ICommand Command => _command;

        public override KeyGesture Gesture => KeyGesture.Parse("F6");
    }
}