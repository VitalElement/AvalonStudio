using System.ComponentModel.Composition;
using AvalonStudio.Extensibility.ToolBars;

namespace AvalonStudio.Shell.Commands
{
	public static class ToolBarDefinitions
	{
		[Export] public static ToolBarItemGroupDefinition StandardOpenSaveToolBarGroup = new ToolBarItemGroupDefinition(
			Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 1);

		[Export] public static ToolBarItemDefinition SaveFileToolBarItem = new CommandToolBarItemDefinition
			<SaveFileCommandDefinition>(
			StandardOpenSaveToolBarGroup, 2);

		[Export] public static ToolBarItemDefinition SaveAllToolBarItem = new CommandToolBarItemDefinition
			<SaveAllFileCommandDefinition>(
			StandardOpenSaveToolBarGroup, 3);

		[Export] public static ToolBarItemGroupDefinition StandardEditGroup = new ToolBarItemGroupDefinition(
			Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 2);

		[Export] public static ToolBarItemDefinition UndoToolBarItem = new CommandToolBarItemDefinition<UndoCommandDefinition>
			(
			StandardEditGroup, 0);

		[Export] public static ToolBarItemDefinition RedoToolBarItem = new CommandToolBarItemDefinition<RedoCommandDefinition>
			(
			StandardEditGroup, 1);

		[Export] public static ToolBarItemDefinition CommentToolBarItem = new CommandToolBarItemDefinition
			<CommentCommandDefinition>(
			StandardEditGroup, 2);

		[Export] public static ToolBarItemDefinition UnCommentToolBarItem = new CommandToolBarItemDefinition
			<UnCommentCommandDefinition>(
			StandardEditGroup, 3);

		[Export] public static ToolBarItemGroupDefinition StandardBuildGroup = new ToolBarItemGroupDefinition(
			Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 3);

		[Export] public static ToolBarItemDefinition BuildToolBarItem = new CommandToolBarItemDefinition
			<BuildCommandDefinition>(
			StandardBuildGroup, 0);

		[Export] public static ToolBarItemDefinition CleanToolBarItem = new CommandToolBarItemDefinition
			<CleanCommandDefinition>(
			StandardBuildGroup, 1);
	}
}