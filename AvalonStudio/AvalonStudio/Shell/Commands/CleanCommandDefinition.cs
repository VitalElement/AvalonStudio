using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class CleanCommandDefinition : CommandDefinition
    {
        private ReactiveCommand _command;

        public override void Activation()
        {
            var shell = IoC.Get<IShell>();

            _command = ReactiveCommand.Create(shell.Clean, shell.CanRunTask());
        }

        public override string Text => "Clean";

        public override string ToolTip => "Cleans the Startup Project.";

        public override DrawingGroup Icon => this.GetCommandIcon("Clean");

        public override ICommand Command => _command;

        public override KeyGesture Gesture => null;
    }
}