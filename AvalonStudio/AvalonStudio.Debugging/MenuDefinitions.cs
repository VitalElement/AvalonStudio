using AvalonStudio.Debugging.Commands;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Debugging
{
	internal static class MenuDefinitions
	{
		[Menu] public static MenuDefinition DebugMenu = new MenuDefinition(
			Extensibility.MainMenu.MenuDefinitions.MainMenuBar, 6, "Debug");

		[MenuGroup] public static MenuItemGroupDefinition DebugStartGroup = new MenuItemGroupDefinition(DebugMenu, 1);

		[MenuItem] public static MenuItemDefinition StartDebugging =
			new CommandMenuItemDefinition<StartDebuggingCommandDefinition>(DebugStartGroup, 0);

		[MenuItem] public static MenuItemDefinition Restart =
			new CommandMenuItemDefinition<RestartDebuggingCommandDefinition>(DebugStartGroup, 2);

		[MenuItem] public static MenuItemDefinition StopDebugging =
			new CommandMenuItemDefinition<StopDebuggingCommandDefinition>(DebugStartGroup, 3);

		[MenuGroup] public static MenuItemGroupDefinition DebugControlGroup = new MenuItemGroupDefinition(DebugMenu, 3);

		[MenuItem] public static MenuItemDefinition StepOver =
			new CommandMenuItemDefinition<StepOverCommandDefinition>(DebugControlGroup, 0);

		[MenuItem] public static MenuItemDefinition StepInto =
			new CommandMenuItemDefinition<StepIntoCommandDefinition>(DebugControlGroup, 1);

		[MenuItem] public static MenuItemDefinition StepIntruction =
			new CommandMenuItemDefinition<StepInstructionCommandDefinition>(DebugControlGroup, 2);

		[MenuItem] public static MenuItemDefinition Pause =
			new CommandMenuItemDefinition<PauseDebuggingCommandDefinition>(DebugControlGroup, 3);
	}
}