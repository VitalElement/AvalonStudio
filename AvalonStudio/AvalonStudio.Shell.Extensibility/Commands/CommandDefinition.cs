using Avalonia.Media;
using System.Windows.Input;

namespace AvalonStudio.Commands
{
    public class CommandDefinition
    {
        public virtual string Label { get; }
        public virtual DrawingGroup Icon { get; }

        public virtual ICommand Command { get; }

        public CommandDefinition(string label, DrawingGroup icon, ICommand command)
        {
            Label = label;
            Icon = icon;

            Command = command;
        }
    }
}
