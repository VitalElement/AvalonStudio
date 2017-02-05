using System.Windows.Input;
using Avalonia.Controls.Shapes;
using AvalonStudio.Extensibility.Commands;

namespace AvalonStudio.Extensibility.ToolBars.Models
{
    public class CommandToolBarItem : ToolBarItemBase
    {
        private readonly ICommand _command;
        private readonly Path _iconPath;
        private readonly ToolBarItemDefinition _toolBarItem;

        public CommandToolBarItem(ToolBarItemDefinition toolBarItem, CommandDefinition commandDefinition)
        {
            _toolBarItem = toolBarItem;
            _command = commandDefinition.Command;
            _iconPath = commandDefinition.IconPath;
        }

        public ToolBarItemDisplay Display => _toolBarItem.Display;

        public Path IconPath => _iconPath;

        public ICommand Command => _command;
    }
}