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

        public static readonly ToolBarItemGroupDefinition StandardOpenSaveToolBarGroup = new ToolBarItemGroupDefinition(
           Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 1);

        public static readonly ToolBarItemDefinition SaveFileToolBarItem = new ToolBarItemDefinition
            <SaveFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 2);

        public static readonly ToolBarItemDefinition SaveAllToolBarItem = new ToolBarItemDefinition
            <SaveAllFileCommandDefinition>(
            StandardOpenSaveToolBarGroup, 3);

        public static readonly ToolBarItemGroupDefinition StandardEditGroup = new ToolBarItemGroupDefinition(
            Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 2);

        public static readonly ToolBarItemDefinition UndoToolBarItem = new ToolBarItemDefinition<UndoCommandDefinition>
            (
            StandardEditGroup, 0);

        public static readonly ToolBarItemDefinition RedoToolBarItem = new ToolBarItemDefinition<RedoCommandDefinition>
            (
            StandardEditGroup, 1);

        public static readonly ToolBarItemDefinition CommentToolBarItem = new ToolBarItemDefinition
            <CommentCommandDefinition>(
            StandardEditGroup, 2);

        public static readonly ToolBarItemDefinition UnCommentToolBarItem = new ToolBarItemDefinition
            <UnCommentCommandDefinition>(
            StandardEditGroup, 3);

        public static readonly ToolBarItemGroupDefinition StandardBuildGroup = new ToolBarItemGroupDefinition(
            Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 3);

        public static readonly ToolBarItemDefinition BuildToolBarItem = new ToolBarItemDefinition
            <BuildCommandDefinition>(
            StandardBuildGroup, 0);

        public static readonly ToolBarItemDefinition CleanToolBarItem = new ToolBarItemDefinition
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