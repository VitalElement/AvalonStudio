using AvalonStudio.Controls.Standard.AboutScreen.Commands;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Controls.Standard.AboutScreen
{
    static class MenuDefinitions
    {
        [MenuGroup]
        public static MenuItemGroupDefinition HelpAboutGroup = new MenuItemGroupDefinition(AvalonStudio.Extensibility.MainMenu.MenuDefinitions.HelpMenu, 300);

        [MenuItem]
        public static MenuItemDefinition HelpAboutItem = new CommandMenuItemDefinition<AboutScreenCommandDefinition>(HelpAboutGroup, 300);

    }
}
