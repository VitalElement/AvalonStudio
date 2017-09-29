using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class CopyCommandDefinition : CommandDefinition
    {
        private ReactiveCommand _command;
        public override string Text => "Copy";

        public override string ToolTip => "Copy ToolTip";
        public override ICommand Command => _command;
    }
}