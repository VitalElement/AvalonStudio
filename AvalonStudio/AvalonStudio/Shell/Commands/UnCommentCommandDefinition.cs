using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class UnCommentCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand _command;

        public UnCommentCommandDefinition()
        {
            _command = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();

                (shell.SelectedDocument as EditorViewModel)?.UnComment();
            });
        }

        public override string Text => "Un-Comment";

        public override string ToolTip => "UnComments the selcted code.";

        public override DrawingGroup Icon => this.GetCommandIcon("UncommentCode");

        public override ICommand Command => _command;
    }
}