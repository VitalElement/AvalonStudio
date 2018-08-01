using AvalonStudio.Menus;
using AvalonStudio.MainMenu;
using System.Composition;

namespace AvalonStudio.Shell.MainMenu
{
    internal class ToolsMainMenuItems
    {
        [ExportMainMenuItem("Tools")]
        [DefaultOrder(150)]
        public IMenuItem Tools => _menuItemFactory.CreateHeaderMenuItem("Tools", null);

        [ExportMainMenuDefaultGroup("Tools", "Extensions")]
        [DefaultOrder(0)]
        public object ExtensionsGroup => null;

        [ExportMainMenuItem("Tools", "Packages")]
        [DefaultOrder(0)]
        [DefaultGroup("Extensions")]
        public IMenuItem Packages => _menuItemFactory.CreateCommandMenuItem("Tools.Packages");

        [ExportMainMenuItem("Tools", "Extensions")]
        [DefaultOrder(50)]
        [DefaultGroup("Extensions")]
        public IMenuItem Extensions => _menuItemFactory.CreateCommandMenuItem("Tools.Extensions");

        [ExportMainMenuDefaultGroup("Tools", "Settings")]
        [DefaultOrder(50)]
        public object SettingsGroup => null;

        [ExportMainMenuItem("Tools", "Options")]
        [DefaultOrder(0)]
        [DefaultGroup("Settings")]
        public IMenuItem Options => _menuItemFactory.CreateCommandMenuItem("Tools.Options");

        private readonly IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public ToolsMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
