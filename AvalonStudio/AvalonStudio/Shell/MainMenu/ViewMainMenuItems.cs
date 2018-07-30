using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Shell.Menus
{
    internal class ViewMainMenuItems
    {
        [ExportMainMenuItem("View")]
        [DefaultOrder(30)]
        public IMenuItem File => _menuItemFactory.CreateHeaderMenuItem("View", null);

        [ExportMainMenuDefaultGroup("View", "DefaultTools")]
        [DefaultOrder(0)]
        public object DefaultToolsGroup => null;

        private IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public ViewMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
