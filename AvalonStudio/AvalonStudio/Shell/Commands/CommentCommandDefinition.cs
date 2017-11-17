using Avalonia.Controls.Shapes;
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
    public class CommentCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand _command;

        public CommentCommandDefinition()
        {
            _command = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();

                (shell.SelectedDocument as IFileDocumentTabViewModel)?.Editor?.Comment();
            });
        }

        public override string Text => "Comment";

        public override string ToolTip => "Comments the selected code.";

        public override DrawingGroup Icon => this.GetCommandIcon("CommentCode");

        public override ICommand Command => _command;
    }
}