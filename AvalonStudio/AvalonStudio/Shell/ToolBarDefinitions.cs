using System.Composition;
using AvalonStudio.Extensibility.ToolBars;
using AvalonStudio.Extensibility.Plugin;
using System;

namespace AvalonStudio.Shell.Commands
{
    public class ToolBarDefinitions : IExtension
    {
        static ToolBarDefinitions()
        {

        }

        public static ToolBarItemGroupDefinition StandardOpenSaveToolBarGroup = new ToolBarItemGroupDefinition(
           Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 1);

        public static ToolBarItemDefinition SaveFileToolBarItem = new ToolBarItemDefinition
            <SaveFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 2);

        public static ToolBarItemDefinition SaveAllToolBarItem = new ToolBarItemDefinition
            <SaveAllFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 3);

        public static ToolBarItemGroupDefinition StandardEditGroup = new ToolBarItemGroupDefinition(
            Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 2);

        public static ToolBarItemDefinition UndoToolBarItem = new ToolBarItemDefinition<UndoCommandDefinition>
            (
            StandardEditGroup, 0);

        public static ToolBarItemDefinition RedoToolBarItem = new ToolBarItemDefinition<RedoCommandDefinition>
            (
            StandardEditGroup, 1);

        public static ToolBarItemDefinition CommentToolBarItem = new ToolBarItemDefinition
            <CommentCommandDefinition>(
            StandardEditGroup, 2);

        public static ToolBarItemDefinition UnCommentToolBarItem = new ToolBarItemDefinition
            <UnCommentCommandDefinition>(
            StandardEditGroup, 3);

        public static ToolBarItemGroupDefinition StandardBuildGroup = new ToolBarItemGroupDefinition(
            Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 3);

        public static ToolBarItemDefinition BuildToolBarItem = new ToolBarItemDefinition
            <BuildCommandDefinition>(
            StandardBuildGroup, 0);

        public static ToolBarItemDefinition CleanToolBarItem = new ToolBarItemDefinition
            <CleanCommandDefinition>(
            StandardBuildGroup, 1);

        public void BeforeActivation()
        {

        }

        public void Activation()
        {

        }
    }
}