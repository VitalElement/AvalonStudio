using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Shell.MainMenu
{
    internal class EditMainMenuItems
    {
        [ExportMainMenuItem("Edit")]
        [DefaultOrder(0)]
        public IMenuItem Edit => _menuItemFactory.CreateHeaderMenuItem("Edit", null);

        //[ExportMainMenuItem("Edit", "Show Command Bar")]
        //[DefaultOrder(0)]        
        //public IMenuItem ShowCommandBar => _menuItemFactory.CreateCommandMenuItem("Edit.ShowQuickCommander");

        [ExportMainMenuItem("Edit", "Undo")]
        [DefaultOrder(0)]
        public IMenuItem Undo => _menuItemFactory.CreateCommandMenuItem("Edit.Undo");

        [ExportMainMenuItem("Edit", "Redo")]
        [DefaultOrder(0)]
        public IMenuItem Redo => _menuItemFactory.CreateCommandMenuItem("Edit.Redo");

        [ExportMainMenuItem("Edit", "Comment")]
        [DefaultOrder(0)]
        public IMenuItem Comment => _menuItemFactory.CreateCommandMenuItem("Edit.Comment");

        [ExportMainMenuItem("Edit", "Uncomment")]
        [DefaultOrder(0)]
        public IMenuItem Uncomment => _menuItemFactory.CreateCommandMenuItem("Edit.Uncomment");

        private IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public EditMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
