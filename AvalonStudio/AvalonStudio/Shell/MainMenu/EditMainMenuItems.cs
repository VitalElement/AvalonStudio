using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace AvalonStudio.Shell.MainMenu
{
    internal class EditMainMenuItems
    {
        [ExportMainMenuItem("Edit")]
        [DefaultOrder(0)]
        public IMenuItem Edit => _menuItemFactory.CreateHeaderMenuItem("Edit", null);

       

        [ExportMainMenuItem("Edit", "Show Command Bar")]
        [DefaultOrder(0)]        
        public IMenuItem ShowCommandBar => _menuItemFactory.CreateCommandMenuItem("Edit.ShowQuickCommander");

        [ExportMainMenuItem("Edit", "Undo")]
        [DefaultOrder(0)]
        public IMenuItem Undo => _menuItemFactory.CreateCommandMenuItem("Edit.Undo");

        [ExportMainMenuItem("Edit", "Redo")]
        [DefaultOrder(0)]
        public IMenuItem Redo => _menuItemFactory.CreateCommandMenuItem("Edit.Redo");

        private IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public EditMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
