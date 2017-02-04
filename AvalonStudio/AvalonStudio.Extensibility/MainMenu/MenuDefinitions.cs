using AvalonStudio.Extensibility.Menus;
using System.Composition;
using System;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Extensibility
{
    public class MenuDefinitions : IExtension
    {
        public static MenuItemGroupDefinition FileNewOpenMenuGroup = new MenuItemGroupDefinition(MainMenu.MenuDefinitions.FileMenu, 0);

        public void Activation()
        {

        }

        public void BeforeActivation()
        {

        }

        //public static MenuItemGroupDefinition FileCloseMenuGroup = new MenuItemGroupDefinition(FileMenu, 3);

        //public static MenuItemGroupDefinition FileSaveMenuGroup = new MenuItemGroupDefinition(FileMenu, 6);

        //public static MenuItemGroupDefinition FileExitOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 10);

        //public static MenuDefinition EditMenu = new MenuDefinition(MainMenuBar, 1, "_Edit");

        //public static MenuItemGroupDefinition EditUndoRedoMenuGroup = new MenuItemGroupDefinition(EditMenu, 0);

        //public static MenuDefinition ViewMenu = new MenuDefinition(MainMenuBar, 2, "_View");

        //public static MenuItemGroupDefinition ViewToolsMenuGroup = new MenuItemGroupDefinition(ViewMenu, 0);

        //public static MenuItemGroupDefinition ViewPropertiesMenuGroup = new MenuItemGroupDefinition(ViewMenu, 100);

        //public static MenuDefinition ToolsMenu = new MenuDefinition(MainMenuBar, 10, "_Tools");

        //public static MenuItemGroupDefinition ToolsOptionsMenuGroup = new MenuItemGroupDefinition(ToolsMenu, 100);

        //public static MenuDefinition WindowMenu = new MenuDefinition(MainMenuBar, 20, "_Window");

        //public static MenuItemGroupDefinition WindowDocumentListMenuGroup = new MenuItemGroupDefinition(WindowMenu, 10);

        //public static MenuDefinition HelpMenu = new MenuDefinition(MainMenuBar, 30, "_Help");

    }
}