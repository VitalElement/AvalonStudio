using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Shell.Menus
{
    internal class HelpMainMenuItems
    {
        [ExportMainMenuItem("Help")]
        [DefaultOrder(1000)]
        public IMenuItem Help => _menuItemFactory.CreateHeaderMenuItem("Help", null);

        [ExportMainMenuDefaultGroup("Help", "About")]
        [DefaultOrder(0)]
        public object AboutGroup => null;

        [ExportMainMenuItem("Help", "About")]
        [DefaultOrder(0)]
        [DefaultGroup("About")]
        public IMenuItem About => _menuItemFactory.CreateCommandMenuItem("Help.About");

        private IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public HelpMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
