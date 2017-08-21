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
    public class SaveFileCommandDefinition : CommandDefinition
    {
        public override KeyGesture Gesture => KeyGesture.Parse("CTRL+S");

        private readonly ReactiveCommand<object> _command;

        public SaveFileCommandDefinition()
        {
            _command = ReactiveCommand.Create();

            _command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();
                shell?.Save();
            });
        }

        public override string Text => "Save";

        public override string ToolTip => "Saves the current changes.";

        public override DrawingGroup Icon => this.GetCommandIcon("Save");

        public override ICommand Command => _command;
    }
}