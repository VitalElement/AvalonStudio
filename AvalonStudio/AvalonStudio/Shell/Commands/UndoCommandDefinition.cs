using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class UndoCommandDefinition : CommandDefinition
    {
        public override KeyGesture Gesture => KeyGesture.Parse("CTRL+Z");

        private readonly ReactiveCommand<object> _command;

        public UndoCommandDefinition()
        {
            _command = ReactiveCommand.Create();

            _command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                (shell.SelectedDocument as EditorViewModel)?.Undo();
            });
        }

        public override string Text => "Undo";

        public override string ToolTip => "Undo the last change.";

        public override DrawingGroup Icon => this.GetCommandIcon("Undo");

        public override ICommand Command => _command;
    }
}