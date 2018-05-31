using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Controls.Standard.FindInFiles.MainMenu
{
    internal class FindInFilesMainMenuItems
    {
        [ExportMainMenuDefaultGroup("Edit", "Find")]
        [DefaultOrder(300)]
        public object FindGroup => null;

        [ExportMainMenuItem("Edit", "FindInFiles")]
        [DefaultGroup("Find")]
        [DefaultOrder(0)]
        public IMenuItem NewSolution => _menuItemFactory.CreateCommandMenuItem("Edit.FindInFiles");

        private readonly IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public FindInFilesMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
