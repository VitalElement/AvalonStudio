using Avalonia.Controls.Shapes;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class CloseFileCommandDefinition : CommandDefinition
    {
        private ReactiveCommand<object> _command;
        public override string Text => "Close";

        public override string ToolTip => "Close ToolTip";
        public override ICommand Command => _command;
        
        public override KeyGesture Gesture => null;
    }
}