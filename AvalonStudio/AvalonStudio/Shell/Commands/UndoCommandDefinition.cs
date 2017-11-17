using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Controls;
using AvalonStudio.Documents;
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

        private readonly ReactiveCommand _command;

        public UndoCommandDefinition()
        {
            _command = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();

                (shell.SelectedDocument as IFileDocumentTabViewModel)?.Editor?.Undo();
            });
        }

        public override string Text => "Undo";

        public override string ToolTip => "Undo the last change.";

        public override DrawingGroup Icon => this.GetCommandIcon("Undo");

        public override ICommand Command => _command;
    }
}