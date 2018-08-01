using System.Composition;
using AvalonStudio.Menus;
using AvalonStudio.Toolbars;

namespace AvalonStudio.Debugging.Toolbars
{
    [Shared]
    internal class DebugToolbarItems
    {
        [ExportToolbarDefaultGroup("Standard", "Debugging")]
        public object DebuggingGroup { get; }

        [ExportToolbarItem("Standard", "Start")]
        [DefaultGroup("Debugging")]
        [DefaultOrder(0)]
        public IMenuItem Start => _menuItemFactory.CreateCommandMenuItem("Debug.Start");

        [ExportToolbarItem("Standard", "Pause")]
        [DefaultGroup("Debugging")]
        [DefaultOrder(50)]
        public IMenuItem Pause => _menuItemFactory.CreateCommandMenuItem("Debug.Pause");

        [ExportToolbarItem("Standard", "Stop")]
        [DefaultGroup("Debugging")]
        [DefaultOrder(100)]
        public IMenuItem Stop => _menuItemFactory.CreateCommandMenuItem("Debug.Stop");

        [ExportToolbarItem("Standard", "Restart")]
        [DefaultGroup("Debugging")]
        [DefaultOrder(150)]
        public IMenuItem Restart => _menuItemFactory.CreateCommandMenuItem("Debug.Restart");

        [ExportToolbarItem("Standard", "Step Over")]
        [DefaultGroup("Debugging")]
        [DefaultOrder(200)]
        public IMenuItem StepOver => _menuItemFactory.CreateCommandMenuItem("Debug.StepOver");

        [ExportToolbarItem("Standard", "Step Into")]
        [DefaultGroup("Debugging")]
        [DefaultOrder(250)]
        public IMenuItem StepInto => _menuItemFactory.CreateCommandMenuItem("Debug.StepInto");

        [ExportToolbarItem("Standard", "Step Out")]
        [DefaultGroup("Debugging")]
        [DefaultOrder(300)]
        public IMenuItem StepOut => _menuItemFactory.CreateCommandMenuItem("Debug.StepOut");

        private readonly IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public DebugToolbarItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
