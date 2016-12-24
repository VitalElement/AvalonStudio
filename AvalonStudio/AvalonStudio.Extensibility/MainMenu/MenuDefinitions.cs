using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Extensibility.MainMenu
{
	public static class MenuDefinitions
	{
		[MenuBar] public static MenuBarDefinition MainMenuBar = new MenuBarDefinition();

		[Menu] public static MenuDefinition FileMenu = new MenuDefinition(MainMenuBar, 0, "_File");

		[MenuGroup] public static MenuItemGroupDefinition FileNewOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 0);

		[MenuGroup] public static MenuItemGroupDefinition FileCloseMenuGroup = new MenuItemGroupDefinition(FileMenu, 3);

		[MenuGroup] public static MenuItemGroupDefinition FileSaveMenuGroup = new MenuItemGroupDefinition(FileMenu, 6);

		[MenuGroup] public static MenuItemGroupDefinition FileExitOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 10);

		[Menu] public static MenuDefinition EditMenu = new MenuDefinition(MainMenuBar, 1, "_Edit");

		[MenuGroup] public static MenuItemGroupDefinition EditUndoRedoMenuGroup = new MenuItemGroupDefinition(EditMenu, 0);

		[Menu] public static MenuDefinition ViewMenu = new MenuDefinition(MainMenuBar, 2, "_View");

		[MenuGroup] public static MenuItemGroupDefinition ViewToolsMenuGroup = new MenuItemGroupDefinition(ViewMenu, 0);

		[MenuGroup] public static MenuItemGroupDefinition ViewPropertiesMenuGroup = new MenuItemGroupDefinition(ViewMenu, 100);

		[Menu] public static MenuDefinition ToolsMenu = new MenuDefinition(MainMenuBar, 10, "_Tools");

		[MenuGroup] public static MenuItemGroupDefinition ToolsOptionsMenuGroup = new MenuItemGroupDefinition(ToolsMenu, 100);

		[Menu] public static MenuDefinition WindowMenu = new MenuDefinition(MainMenuBar, 20, "_Window");

		[MenuGroup] public static MenuItemGroupDefinition WindowDocumentListMenuGroup = new MenuItemGroupDefinition(
			WindowMenu, 10);

		[Menu] public static MenuDefinition HelpMenu = new MenuDefinition(MainMenuBar, 30, "_Help");
	}
}