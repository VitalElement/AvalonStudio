using AvalonStudio.Extensibility.Menus;
using System.Windows.Input;

namespace AvalonStudio.Extensibility.MainMenu.Models
{
    public class TextMenuItem : StandardMenuItem
    {
        private readonly MenuDefinition _menuDefinition;

        public TextMenuItem(MenuDefinition menuDefinition)
        {
            _menuDefinition = menuDefinition;
        }

        public override string Text
        {
            get { return _menuDefinition.Text; }
        }

        public override ICommand Command
        {
            get { return null; }
        }

        public override bool IsChecked
        {
            get { return false; }
        }

        public override bool IsVisible
        {
            get { return true; }
        }
    }
}