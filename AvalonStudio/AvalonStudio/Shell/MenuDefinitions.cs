using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Shell.Commands;
using AvalonStudio.Extensibility.MainMenu;

namespace AvalonStudio.Shell
{
    internal class DefaultMenuDefinitions
    {
        //[MenuItem]
        //public static MenuItemDefinition FileNewMenuItemList = new CommandMenuItemDefinition<NewFileCommandDefinition>(
        //    FileNewCascadeGroup, 0);

        //[MenuGroup]
        //public static MenuItemGroupDefinition FileNewCascadeGroup = new MenuItemGroupDefinition(
        //    FileNewMenuItem, 0);

        [Menu]
        public static MenuDefinition ToolsMenu = new MenuDefinition(
            Extensibility.MainMenu.MenuDefinitions.MainMenuBar, 8, "Tools");

        [Menu]
        public static MenuDefinition EditMenu = new MenuDefinition(MenuDefinitions.MainMenuBar, 2, "Edit");

        [MenuGroup]
        public static MenuItemGroupDefinition EditFindGroup = new MenuItemGroupDefinition(EditMenu, 1);

        [MenuItem]
        public static MenuItemDefinition QuickFindMenuItem = new CommandMenuItemDefinition<QuickFindCommandDefinition>(EditFindGroup, 1);

        [MenuGroup]
        public static MenuItemGroupDefinition ToolsPackagesMenuGroup = new MenuItemGroupDefinition(ToolsMenu, 6);

        [MenuItem]
        public static MenuItemDefinition ToolsPackagesMenuItems = new CommandMenuItemDefinition<PackagesCommandDefinition>(ToolsPackagesMenuGroup, 1);

        [MenuItem]
        public static MenuItemDefinition FileSaveAllMenuItem = new CommandMenuItemDefinition
            <SaveAllFileCommandDefinition>(
            Extensibility.MainMenu.MenuDefinitions.FileSaveMenuGroup, 1);

        [MenuItem]
        public static MenuItemDefinition FileSaveMenuItem = new CommandMenuItemDefinition
            <SaveFileCommandDefinition>(
            Extensibility.MainMenu.MenuDefinitions.FileSaveMenuGroup, 0);

        [MenuItem]
        public static MenuItemDefinition FileExitMenuItem = new CommandMenuItemDefinition<ExitCommandDefinition>(
            Extensibility.MainMenu.MenuDefinitions.FileExitOpenMenuGroup, 0);
    }
}