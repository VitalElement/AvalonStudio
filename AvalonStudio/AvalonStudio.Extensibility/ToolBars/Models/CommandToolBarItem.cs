using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Extensibility.Commands;
using System.Windows.Input;

namespace AvalonStudio.Extensibility.ToolBars.Models
{
    public class CommandToolBarItem : ToolBarItemBase
    {
        private readonly ICommand _command;
        private readonly DrawingGroup _icon;
        private readonly ToolBarItemDefinition _toolBarItem;

        public CommandToolBarItem(ToolBarItemDefinition toolBarItem, CommandDefinition commandDefinition)
        {
            _toolBarItem = toolBarItem;
            _command = commandDefinition.Command;
            _icon = commandDefinition.Icon;
        }

        public ToolBarItemDisplay Display => _toolBarItem.Display;

        public DrawingGroup Icon => _icon;

        public ICommand Command => _command;
    }
}