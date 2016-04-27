using AvalonStudio.Extensibility.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.MainMenu
{
    public static class MenuDefinitions
    {
        [MenuDefinition]
        public static MenuBarDefinition MainMenuBar = new MenuBarDefinition();

        [MenuDefinition]
        public static MenuDefinition FileMenu = new MenuDefinition(MainMenuBar, 0, "_File");

        [MenuDefinition]
        public static MenuItemGroupDefinition FileNewOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 0);

        [MenuDefinition]
        public static MenuItemGroupDefinition FileCloseMenuGroup = new MenuItemGroupDefinition(FileMenu, 3);

        [MenuDefinition]
        public static MenuItemGroupDefinition FileSaveMenuGroup = new MenuItemGroupDefinition(FileMenu, 6);

        [MenuDefinition]
        public static MenuItemGroupDefinition FileExitOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 10);

        [MenuDefinition]
        public static MenuDefinition EditMenu = new MenuDefinition(MainMenuBar, 1, "_Edit");

        [MenuDefinition]
        public static MenuItemGroupDefinition EditUndoRedoMenuGroup = new MenuItemGroupDefinition(EditMenu, 0);

        [MenuDefinition]
        public static MenuDefinition ViewMenu = new MenuDefinition(MainMenuBar, 2, "_View");

        [MenuDefinition]
        public static MenuItemGroupDefinition ViewToolsMenuGroup = new MenuItemGroupDefinition(ViewMenu, 0);

        [MenuDefinition]
        public static MenuItemGroupDefinition ViewPropertiesMenuGroup = new MenuItemGroupDefinition(ViewMenu, 100);

        [MenuDefinition]
        public static MenuDefinition ToolsMenu = new MenuDefinition(MainMenuBar, 10, "_Tools");

        [MenuDefinition]
        public static MenuItemGroupDefinition ToolsOptionsMenuGroup = new MenuItemGroupDefinition(ToolsMenu, 100);

        [MenuDefinition]
        public static MenuDefinition WindowMenu = new MenuDefinition(MainMenuBar, 20, "_Window");

        [MenuDefinition]
        public static MenuItemGroupDefinition WindowDocumentListMenuGroup = new MenuItemGroupDefinition(WindowMenu, 10);

        [MenuDefinition]
        public static MenuDefinition HelpMenu = new MenuDefinition(MainMenuBar, 30, "_Help");
    }
}
