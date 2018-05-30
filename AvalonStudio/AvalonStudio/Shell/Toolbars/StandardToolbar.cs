using AvalonStudio.Toolbars;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Shell.Toolbars
{
    internal class StandardToolbar
    {
        [ExportToolbar("Standard")]
        public Toolbar Standard => new Toolbar("Standard");

        [ExportToolbarDefaultGroup("Standard", "Save")]
        [DefaultOrder(0)]
        public object NewOpenSaveGroup => null;

        [ExportToolbarItem("Standard", "Save")]
        [DefaultGroup("Save")]
        [DefaultOrder(0)]
        public IMenuItem Save => _menuItemFactory.CreateCommandMenuItem("File.Save");

        [ExportToolbarItem("Standard", "SaveAll")]
        [DefaultGroup("Save")]
        [DefaultOrder(50)]
        public IMenuItem SaveAll => _menuItemFactory.CreateCommandMenuItem("File.SaveAll");

        [ExportToolbarDefaultGroup("Standard", "Edit")]
        [DefaultOrder(50)]
        public object EditGroup => null;

        [ExportToolbarItem("Standard", "Undo")]
        [DefaultGroup("Edit")]
        [DefaultOrder(0)]
        public IMenuItem Undo => _menuItemFactory.CreateCommandMenuItem("Edit.Undo");

        [ExportToolbarItem("Standard", "Redo")]
        [DefaultGroup("Edit")]
        [DefaultOrder(50)]
        public IMenuItem Redo => _menuItemFactory.CreateCommandMenuItem("Edit.Redo");

        [ExportToolbarItem("Standard", "Comment")]
        [DefaultGroup("Edit")]
        [DefaultOrder(100)]
        public IMenuItem Comment => _menuItemFactory.CreateCommandMenuItem("Edit.Comment");

        [ExportToolbarItem("Standard", "Uncomment")]
        [DefaultGroup("Edit")]
        [DefaultOrder(150)]
        public IMenuItem Uncomment => _menuItemFactory.CreateCommandMenuItem("Edit.Uncomment");

        [ExportToolbarDefaultGroup("Standard", "Build")]
        [DefaultOrder(100)]
        public object BuildGroup => null;

        [ExportToolbarItem("Standard", "Build")]
        [DefaultGroup("Build")]
        [DefaultOrder(0)]
        public IMenuItem Build => _menuItemFactory.CreateCommandMenuItem("Build.Build");

        [ExportToolbarItem("Standard", "Clean")]
        [DefaultGroup("Build")]
        [DefaultOrder(50)]
        public IMenuItem Clean => _menuItemFactory.CreateCommandMenuItem("Build.Clean");

        private readonly IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public StandardToolbar(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
