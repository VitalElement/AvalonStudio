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

        [DefaultOrder(500)]
        [ExportMainMenuDefaultGroup("File", "Save")]
        public object SaveGroup => null;

        [ExportMainMenuItem("File", "Save")]
        [DefaultOrder(0)]
        [DefaultGroup("Save")]
        public IMenuItem Save => _menuItemFactory.CreateCommandMenuItem("File.Save");

        [ExportMainMenuItem("File", "SaveAll")]
        [DefaultOrder(1)]
        [DefaultGroup("Save")]
        public IMenuItem SaveAll => _menuItemFactory.CreateCommandMenuItem("File.SaveAll");

        [ExportOnPlatform(osx: false)]
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
