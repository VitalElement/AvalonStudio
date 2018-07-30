using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    internal class ErrorListMainMenuItems
    {
        [ExportMainMenuItem("View", "Error List")]
        [DefaultGroup("DefaultTools")]
        [DefaultOrder(0)]
        public IMenuItem ViewErrorList => _menuItemFactory.CreateCommandMenuItem("View.ErrorList");

        private readonly IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public ErrorListMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
