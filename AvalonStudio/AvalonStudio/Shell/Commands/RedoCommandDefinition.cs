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
    public class RedoCommandDefinition : CommandDefinition
    {
        public override KeyGesture Gesture => KeyGesture.Parse("CTRL+Y");

        private readonly ReactiveCommand _command;

        public RedoCommandDefinition()
        {
            _command = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();

                //(shell.SelectedDocument as EditorViewModel)?.Redo();
            });
        }

        public override string Text => "Redo";

        public override string ToolTip => "Redo the last undo command.";

        public override DrawingGroup Icon => this.GetCommandIcon("Redo");

        public override ICommand Command => _command;
    }
}