using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Shell.Commands;

namespace AvalonStudio.Shell
{
	internal class MenuDefinitions
	{
		//[MenuItem]
		//public static MenuItemDefinition FileNewMenuItemList = new CommandMenuItemDefinition<NewFileCommandDefinition>(
		//    FileNewCascadeGroup, 0);

		//[MenuGroup]
		//public static MenuItemGroupDefinition FileNewCascadeGroup = new MenuItemGroupDefinition(
		//    FileNewMenuItem, 0);

		[Menu] public static MenuDefinition ToolsMenu = new MenuDefinition(
			Extensibility.MainMenu.MenuDefinitions.MainMenuBar, 8, "Tools");

		[MenuGroup] public static MenuItemGroupDefinition ToolsPackagesMenuGroup = new MenuItemGroupDefinition(ToolsMenu, 6);

		[MenuItem] public static MenuItemDefinition ToolsPackagesMenuItems = new CommandMenuItemDefinition
			<PackagesCommandDefinition>(
			ToolsPackagesMenuGroup, 1);

		[MenuItem] public static MenuItemDefinition FileSaveAllMenuItem = new CommandMenuItemDefinition
			<SaveAllFileCommandDefinition>(
			Extensibility.MainMenu.MenuDefinitions.FileSaveMenuGroup, 1);

		[MenuItem] public static MenuItemDefinition FileSaveMenuItem = new CommandMenuItemDefinition
			<SaveFileCommandDefinition>(
			Extensibility.MainMenu.MenuDefinitions.FileSaveMenuGroup, 0);

		[MenuItem] public static MenuItemDefinition FileExitMenuItem = new CommandMenuItemDefinition<ExitCommandDefinition>(
			Extensibility.MainMenu.MenuDefinitions.FileExitOpenMenuGroup, 0);
	}
}