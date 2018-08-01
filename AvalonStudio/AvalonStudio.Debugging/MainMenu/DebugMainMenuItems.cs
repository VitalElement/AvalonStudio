using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Debugging.MainMenu
{
    internal class DebugMainMenuItems
    {
        [ExportMainMenuItem("Debug")]
        [DefaultOrder(100)]
        public IMenuItem Debug => _menuItemFactory.CreateHeaderMenuItem("Debug", null);

        [ExportMainMenuItem("Debug", "Start Debugging")]
        [DefaultOrder(0)]
        public IMenuItem StartDebugging => _menuItemFactory.CreateCommandMenuItem("Debug.Start");

        [ExportMainMenuItem("Debug", "Stop")]
        [DefaultOrder(0)]
        public IMenuItem Stop => _menuItemFactory.CreateCommandMenuItem("Debug.Stop");

        [ExportMainMenuItem("Debug", "Pause")]
        [DefaultOrder(0)]
        public IMenuItem Pause => _menuItemFactory.CreateCommandMenuItem("Debug.Pause");

        [ExportMainMenuItem("Debug", "Restart")]
        [DefaultOrder(0)]
        public IMenuItem Restart => _menuItemFactory.CreateCommandMenuItem("Debug.Restart");

        [ExportMainMenuItem("Debug", "Step Over")]
        [DefaultOrder(0)]
        public IMenuItem StepOver => _menuItemFactory.CreateCommandMenuItem("Debug.StepOver");

        [ExportMainMenuItem("Debug", "Step Into")]
        [DefaultOrder(0)]
        public IMenuItem StepInto => _menuItemFactory.CreateCommandMenuItem("Debug.StepInto");

        [ExportMainMenuItem("Debug", "Step Out")]
        [DefaultOrder(0)]
        public IMenuItem StepOut => _menuItemFactory.CreateCommandMenuItem("Debug.StepOut");

        [ExportMainMenuItem("Debug", "Step Instruction")]
        [DefaultOrder(0)]
        public IMenuItem StepInstruction => _menuItemFactory.CreateCommandMenuItem("Debug.StepInstruction");

        private IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public DebugMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
