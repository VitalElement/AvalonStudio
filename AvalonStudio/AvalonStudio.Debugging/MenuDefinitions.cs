namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using AvalonStudio.Controls.Standard.SolutionExplorer.Commands;
    using AvalonStudio.Extensibility.Menus;

    static class MenuDefinitions
    {
        [Menu]        
        public static MenuDefinition DebugMenu = new MenuDefinition(Extensibility.MainMenu.MenuDefinitions.MainMenuBar, 6, "Debug");

        [MenuGroup]
        public static MenuItemGroupDefinition DebugStartGroup = new MenuItemGroupDefinition(DebugMenu, 1);

        [MenuItem]
        public static MenuItemDefinition StartDebugging = new CommandMenuItemDefinition<StartDebuggingCommandDefinition>(DebugStartGroup, 0);
    }
}
