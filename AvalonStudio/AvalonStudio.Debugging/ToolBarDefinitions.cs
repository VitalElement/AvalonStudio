namespace AvalonStudio.Debugging
{
    using AvalonStudio.Extensibility.ToolBars;
    using Commands;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    static class ToolBarDefinitions
    {
        [Export]
        public static ToolBarItemGroupDefinition DebuggingGroup = new ToolBarItemGroupDefinition(
            AvalonStudio.Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 5);

        [Export]
        public static ToolBarItemDefinition StartDebuggingToolBarItem = new CommandToolBarItemDefinition<StartDebuggingCommandDefinition>(
            DebuggingGroup, 1);

        [Export]
        public static ToolBarItemDefinition PauseDebuggingToolBarItem = new CommandToolBarItemDefinition<PauseDebuggingCommandDefinition>(
            DebuggingGroup, 2);

        [Export]
        public static ToolBarItemDefinition StopDebuggingToolBarItem = new CommandToolBarItemDefinition<StopDebuggingCommandDefinition>(
            DebuggingGroup, 3);

        [Export]
        public static ToolBarItemDefinition RestartDebuggingToolBarItem = new CommandToolBarItemDefinition<RestartDebuggingCommandDefinition>(
            DebuggingGroup, 4);

        [Export]
        public static ToolBarItemDefinition StepOverToolBarItem = new CommandToolBarItemDefinition<StepOverCommandDefinition>(
            DebuggingGroup, 5);

        [Export]
        public static ToolBarItemDefinition StepIntoToolBarItem = new CommandToolBarItemDefinition<StepIntoCommandDefinition>(
            DebuggingGroup, 6);

        [Export]
        public static ToolBarItemDefinition StepOutToolBarItem = new CommandToolBarItemDefinition<StepOutCommandDefinition>(
            DebuggingGroup, 7);        
    }
}
