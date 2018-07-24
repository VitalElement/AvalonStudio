using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Controls.Standard.SolutionExplorer.MainMenu
{
    internal class ConsoleMainMenuItems
    {
        [ExportMainMenuItem("View", "Console")]
        [DefaultGroup("DefaultTools")]
        [DefaultOrder(0)]
        public IMenuItem ViewConsole => _menuItemFactory.CreateCommandMenuItem("View.Console");

        private readonly IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public ConsoleMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
