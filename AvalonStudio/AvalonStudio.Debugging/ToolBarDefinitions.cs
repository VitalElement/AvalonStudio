namespace AvalonStudio.Debugging
{
    using AvalonStudio.Debugging.Commands;
    using AvalonStudio.Extensibility.Plugin;
    using AvalonStudio.Extensibility.ToolBars;

    internal class ToolBarDefinitions : IExtension
    {
        public static readonly ToolBarItemGroupDefinition DebuggingGroup = new ToolBarItemGroupDefinition(Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 5);

        public static readonly ToolBarItemDefinition StartDebuggingToolBarItem = new ToolBarItemDefinition<StartDebuggingCommandDefinition>(DebuggingGroup, 1);

        public static readonly ToolBarItemDefinition PauseDebuggingToolBarItem = new ToolBarItemDefinition<PauseDebuggingCommandDefinition>(DebuggingGroup, 2);

        public static readonly ToolBarItemDefinition StopDebuggingToolBarItem = new ToolBarItemDefinition<StopDebuggingCommandDefinition>(DebuggingGroup, 3);

        public static readonly ToolBarItemDefinition RestartDebuggingToolBarItem = new ToolBarItemDefinition<RestartDebuggingCommandDefinition>(DebuggingGroup, 4);

        public static readonly ToolBarItemDefinition StepOverToolBarItem = new ToolBarItemDefinition<StepOverCommandDefinition>(DebuggingGroup, 5);

        public static readonly ToolBarItemDefinition StepIntoToolBarItem = new ToolBarItemDefinition<StepIntoCommandDefinition>(DebuggingGroup, 6);

        public static readonly ToolBarItemDefinition StepOutToolBarItem = new ToolBarItemDefinition<StepOutCommandDefinition>(DebuggingGroup, 7);

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
        }
    }
}