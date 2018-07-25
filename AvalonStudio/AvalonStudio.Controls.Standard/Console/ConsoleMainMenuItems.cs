using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Controls.Standard.Console
{
    internal class ConsoleMainMenuItems
    {
        [ExportMainMenuItem("View", "Console")]
        [DefaultGroup("DefaultTools")]
        [DefaultOrder(0)]
        public IMenuItem ViewConsole => _menuItemFactory.CreateCommandMenuItem("View.Console");

        [ExportMainMenuItem("View", "Welcome Screen")]
        [DefaultGroup("DefaultTools")]
        [DefaultOrder(0)]
        public IMenuItem ViewWelcomeScreen => _menuItemFactory.CreateCommandMenuItem("View.WelcomeScreen");

        private readonly IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public ConsoleMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
