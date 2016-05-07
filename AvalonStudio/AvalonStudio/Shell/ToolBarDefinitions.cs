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

        [Export]
        public static ToolBarItemGroupDefinition StandardEditGroup = new ToolBarItemGroupDefinition(
            MainToolBar, 2);

        [Export]
        public static ToolBarItemDefinition UndoToolBarItem = new CommandToolBarItemDefinition<UndoCommandDefinition>(
            StandardEditGroup, 0);

        [Export]
        public static ToolBarItemDefinition RedoToolBarItem = new CommandToolBarItemDefinition<RedoCommandDefinition>(
            StandardEditGroup, 1);

        [Export]
        public static ToolBarItemDefinition CommentToolBarItem = new CommandToolBarItemDefinition<CommentCommandDefinition>(
            StandardEditGroup, 2);

        [Export]
        public static ToolBarItemDefinition UnCommentToolBarItem = new CommandToolBarItemDefinition<UnCommentCommandDefinition>(
            StandardEditGroup, 3);

        [Export]
        public static ToolBarItemGroupDefinition StandardBuildGroup = new ToolBarItemGroupDefinition(
            MainToolBar, 3);

        [Export]
        public static ToolBarItemDefinition BuildToolBarItem = new CommandToolBarItemDefinition<BuildCommandDefinition>(
            StandardBuildGroup, 0);

        [Export]
        public static ToolBarItemDefinition CleanToolBarItem = new CommandToolBarItemDefinition<CleanCommandDefinition>(
            StandardBuildGroup, 1);
    }
}
