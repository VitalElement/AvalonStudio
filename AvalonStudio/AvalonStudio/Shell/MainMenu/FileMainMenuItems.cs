using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Shell.Menus
{
    internal class FileMainMenuItems
    {
        [ExportMainMenuItem("File")]
        [DefaultOrder(0)]
        public IMenuItem File => _menuItemFactory.CreateHeaderMenuItem("File", null);

        [ExportMainMenuDefaultGroup("File", "Exit")]
        [DefaultOrder(1000)]
        public object ExitGroup => null;

        [ExportMainMenuItem("File", "Exit")]
        [DefaultOrder(0)]
        [DefaultGroup("Exit")]
        public IMenuItem Exit => _menuItemFactory.CreateCommandMenuItem("File.Exit");

        private IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public FileMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
