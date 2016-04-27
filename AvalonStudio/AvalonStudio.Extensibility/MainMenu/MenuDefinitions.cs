using AvalonStudio.Extensibility.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.MainMenu
{
    public static class MenuDefinitions
    {
        [Export]
        public static MenuBarDefinition MainMenuBar = new MenuBarDefinition();

        [Export]
        public static MenuDefinition FileMenu = new MenuDefinition(MainMenuBar, 0, "_File");

        [Export]
        public static MenuItemGroupDefinition FileNewOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 0);

        [Export]
        public static MenuItemGroupDefinition FileCloseMenuGroup = new MenuItemGroupDefinition(FileMenu, 3);

        [Export]
        public static MenuItemGroupDefinition FileSaveMenuGroup = new MenuItemGroupDefinition(FileMenu, 6);

        [Export]
        public static MenuItemGroupDefinition FileExitOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 10);

        [Export]
        public static MenuDefinition EditMenu = new MenuDefinition(MainMenuBar, 1, "_Edit");

        [Export]
        public static MenuItemGroupDefinition EditUndoRedoMenuGroup = new MenuItemGroupDefinition(EditMenu, 0);

        [Export]
        public static MenuDefinition ViewMenu = new MenuDefinition(MainMenuBar, 2, "_View");

        [Export]
        public static MenuItemGroupDefinition ViewToolsMenuGroup = new MenuItemGroupDefinition(ViewMenu, 0);

        [Export]
        public static MenuItemGroupDefinition ViewPropertiesMenuGroup = new MenuItemGroupDefinition(ViewMenu, 100);

        [Export]
        public static MenuDefinition ToolsMenu = new MenuDefinition(MainMenuBar, 10, "_Tools");

        [Export]
        public static MenuItemGroupDefinition ToolsOptionsMenuGroup = new MenuItemGroupDefinition(ToolsMenu, 100);

        [Export]
        public static MenuDefinition WindowMenu = new MenuDefinition(MainMenuBar, 20, "_Window");

        [Export]
        public static MenuItemGroupDefinition WindowDocumentListMenuGroup = new MenuItemGroupDefinition(WindowMenu, 10);

        [Export]
        public static MenuDefinition HelpMenu = new MenuDefinition(MainMenuBar, 30, "_Help");
    }
}
