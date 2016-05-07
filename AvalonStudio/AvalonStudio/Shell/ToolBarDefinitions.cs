namespace AvalonStudio.Shell
{
    using AvalonStudio.Extensibility.ToolBars;
    using Commands;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    static class ToolBarDefinitions
    {
        [Export]
        public static ToolBarDefinition MainToolBar = new ToolBarDefinition(0, "Main");

        [Export]
        public static ToolBarItemGroupDefinition StandardOpenSaveToolBarGroup = new ToolBarItemGroupDefinition(
            MainToolBar, 1);

        [Export]
        public static ToolBarItemDefinition SaveFileToolBarItem = new CommandToolBarItemDefinition<SaveFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 2);

        [Export]
        public static ToolBarItemDefinition SaveAllToolBarItem = new CommandToolBarItemDefinition<SaveAllFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 3);
    }
}
