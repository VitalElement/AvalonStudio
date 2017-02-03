using AvalonStudio.Debugging.Commands;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Debugging
{
	internal static class MenuDefinitions
	{
		public static MenuDefinition DebugMenu = new MenuDefinition(
			Extensibility.MainMenu.MenuDefinitions.MainMenuBar, 6, "Debug");

		 public static MenuItemGroupDefinition DebugStartGroup = new MenuItemGroupDefinition(DebugMenu, 1);

		 public static MenuItemDefinition StartDebugging =
			new CommandMenuItemDefinition<StartDebuggingCommandDefinition>(DebugStartGroup, 0);

		 public static MenuItemDefinition Restart =
			new CommandMenuItemDefinition<RestartDebuggingCommandDefinition>(DebugStartGroup, 2);

		 public static MenuItemDefinition StopDebugging =
			new CommandMenuItemDefinition<StopDebuggingCommandDefinition>(DebugStartGroup, 3);

		 public static MenuItemGroupDefinition DebugControlGroup = new MenuItemGroupDefinition(DebugMenu, 3);

		 public static MenuItemDefinition StepOver =
			new CommandMenuItemDefinition<StepOverCommandDefinition>(DebugControlGroup, 0);

		 public static MenuItemDefinition StepInto =
			new CommandMenuItemDefinition<StepIntoCommandDefinition>(DebugControlGroup, 1);

		 public static MenuItemDefinition StepIntruction =
			new CommandMenuItemDefinition<StepInstructionCommandDefinition>(DebugControlGroup, 2);

		 public static MenuItemDefinition Pause =
			new CommandMenuItemDefinition<PauseDebuggingCommandDefinition>(DebugControlGroup, 3);
	}
}