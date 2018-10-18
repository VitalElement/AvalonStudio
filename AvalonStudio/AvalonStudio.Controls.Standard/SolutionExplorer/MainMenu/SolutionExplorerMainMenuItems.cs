using AvalonStudio.MainMenu;
using AvalonStudio.Menus;
using System.Composition;

namespace AvalonStudio.Controls.Standard.SolutionExplorer.MainMenu
{
    internal class SolutionExplorerMainMenuItems
    {
        [ExportMainMenuDefaultGroup("File", "New/Open")]
        [DefaultOrder(0)]
        public object NewOpenGroup => null;

        [ExportMainMenuItem("File", "New Solution")]
        [DefaultGroup("New/Open")]
        [DefaultOrder(0)]
        public IMenuItem NewSolution => _menuItemFactory.CreateCommandMenuItem("File.NewSolution");

        [ExportMainMenuItem("File", "Open Solution")]
        [DefaultGroup("New/Open")]
        [DefaultOrder(50)]
        public IMenuItem OpenSolution => _menuItemFactory.CreateCommandMenuItem("File.OpenSolution");

        [ExportMainMenuDefaultGroup("File", "Close")]
        [DefaultOrder(50)]
        public object CloseGroup => null;

        [ExportMainMenuItem("File", "Close Solution")]
        [DefaultGroup("Close")]
        [DefaultOrder(100)]
        public IMenuItem CloseSolution => _menuItemFactory.CreateCommandMenuItem("File.CloseSolution");

        [ExportMainMenuItem("View", "Solution Explorer")]
        [DefaultGroup("DefaultTools")]
        [DefaultOrder(0)]
        public IMenuItem ViewSolutionExplorer => _menuItemFactory.CreateCommandMenuItem("View.SolutionExplorer");

        private readonly IMenuItemFactory _menuItemFactory;

        [ImportingConstructor]
        public SolutionExplorerMainMenuItems(IMenuItemFactory menuItemFactory)
        {
            _menuItemFactory = menuItemFactory;
        }
    }
}
