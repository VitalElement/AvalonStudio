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

        [MenuGroup]
        public static MenuItemGroupDefinition DebugControlGroup = new MenuItemGroupDefinition(DebugMenu, 3);

        [MenuItem]
        public static MenuItemDefinition StepOver = new CommandMenuItemDefinition<StepOverCommandDefinition>(DebugControlGroup, 0);

        public static MenuItemDefinition StepInto = new CommandMenuItemDefinition<StepIntoCommandDefinition>(DebugControlGroup, 1);

        public static MenuItemDefinition StepIntruction = new CommandMenuItemDefinition<StepInstructionCommandDefinition>(DebugControlGroup, 2);

        public static MenuItemDefinition Pause = new CommandMenuItemDefinition<PauseDebuggingCommandDefinition>(DebugControlGroup, 3);

        public static MenuItemDefinition Stop = new CommandMenuItemDefinition<StopDebuggingCommandDefinition>(DebugControlGroup, 4);

    }
}
