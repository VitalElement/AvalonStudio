using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Shell.Commands;

namespace AvalonStudio.Shell
{
    internal class MenuDefinitions : IExtension
    {
        static MenuDefinitions()
        {
            // Do Nothing
        }

        public static readonly MenuItemGroupDefinition ToolsPackagesMenuGroup = new MenuItemGroupDefinition(Extensibility.MenuDefinitions.ToolsMenu, 6);

        public static readonly MenuItemDefinition ToolsExtensionsMenuItem = new MenuItemDefinition<ExtensionsCommandDefinition>(ToolsPackagesMenuGroup, 1);

        public static readonly MenuItemDefinition ToolsPackagesMenuItem = new MenuItemDefinition<PackagesCommandDefinition>(ToolsPackagesMenuGroup, 2);

        public static readonly MenuItemGroupDefinition ToolsSettingsMenuGroup = new MenuItemGroupDefinition(Extensibility.MenuDefinitions.ToolsMenu, 8);

        public static readonly MenuItemDefinition ToolsOptionsMenuItem = new MenuItemDefinition<OptionsCommandDefinition>(ToolsSettingsMenuGroup, 1);

        public static readonly MenuItemDefinition FileSaveAllMenuItem = new MenuItemDefinition<SaveAllFileCommandDefinition>(Extensibility.MenuDefinitions.FileSaveMenuGroup, 1);

        public static readonly MenuItemDefinition FileSaveMenuItem = new MenuItemDefinition<SaveFileCommandDefinition>(Extensibility.MenuDefinitions.FileSaveMenuGroup, 0);

        public static readonly MenuItemDefinition FileExitMenuItem = new MenuItemDefinition<ExitCommandDefinition>(Extensibility.MenuDefinitions.FileExitOpenMenuGroup, 0);

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}