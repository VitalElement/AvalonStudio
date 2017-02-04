using AvalonStudio.Debugging.Commands;
using AvalonStudio.Extensibility.ToolBars;

namespace AvalonStudio.Debugging
{
	internal static class ToolBarDefinitions
	{
		 public static ToolBarItemGroupDefinition DebuggingGroup = new ToolBarItemGroupDefinition(
			Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 5);

		 //public static ToolBarItemDefinition StartDebuggingToolBarItem = new CommandToolBarItemDefinition
			//<StartDebuggingCommandDefinition>(
			//DebuggingGroup, 1);

		 //public static ToolBarItemDefinition PauseDebuggingToolBarItem = new CommandToolBarItemDefinition
			//<PauseDebuggingCommandDefinition>(
			//DebuggingGroup, 2);

		 //public static ToolBarItemDefinition StopDebuggingToolBarItem = new CommandToolBarItemDefinition
			//<StopDebuggingCommandDefinition>(
			//DebuggingGroup, 3);

		 //public static ToolBarItemDefinition RestartDebuggingToolBarItem = new CommandToolBarItemDefinition
			//<RestartDebuggingCommandDefinition>(
			//DebuggingGroup, 4);

		 //public static ToolBarItemDefinition StepOverToolBarItem = new CommandToolBarItemDefinition
			//<StepOverCommandDefinition>(
			//DebuggingGroup, 5);

		 //public static ToolBarItemDefinition StepIntoToolBarItem = new CommandToolBarItemDefinition
			//<StepIntoCommandDefinition>(
			//DebuggingGroup, 6);

		 //public static ToolBarItemDefinition StepOutToolBarItem = new CommandToolBarItemDefinition
			//<StepOutCommandDefinition>(
			//DebuggingGroup, 7);
	}
}