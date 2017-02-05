using System;
using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Shell.Commands;

namespace AvalonStudio.Shell
{
	internal class MenuDefinitions : IExtension
	{
        static MenuDefinitions()
        {
        }

        public static MenuItemGroupDefinition ToolsPackagesMenuGroup = new MenuItemGroupDefinition(Extensibility.MenuDefinitions.ToolsMenu, 6);

        public static MenuItemDefinition ToolsPackagesMenuItem = new MenuItemDefinition<PackagesCommandDefinition>(ToolsPackagesMenuGroup, "Packages", 1);

        public static MenuItemDefinition FileSaveAllMenuItem = new MenuItemDefinition<SaveAllFileCommandDefinition>(Extensibility.MenuDefinitions.FileSaveMenuGroup, "SaveAll", 1);

        public static MenuItemDefinition FileSaveMenuItem = new MenuItemDefinition<SaveFileCommandDefinition>(Extensibility.MenuDefinitions.FileSaveMenuGroup, "Save", 0);

        public static MenuItemDefinition FileExitMenuItem = new MenuItemDefinition<ExitCommandDefinition>(Extensibility.MenuDefinitions.FileExitOpenMenuGroup, "Exit", 0);

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            
        }
    }
}