using System;
using AvalonStudio.Debugging.Commands;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility.ToolBars;

namespace AvalonStudio.Debugging
{
    internal class ToolBarDefinitions : IExtension
    {
        static ToolBarDefinitions()
        {

        }

        public static ToolBarItemGroupDefinition DebuggingGroup = new ToolBarItemGroupDefinition(
           Extensibility.MainToolBar.ToolBarDefinitions.MainToolBar, 5);

        public static ToolBarItemDefinition StartDebuggingToolBarItem = new ToolBarItemDefinition<StartDebuggingCommandDefinition>(DebuggingGroup, 1);

        public static ToolBarItemDefinition PauseDebuggingToolBarItem = new ToolBarItemDefinition
           <PauseDebuggingCommandDefinition>(
           DebuggingGroup, 2);

        public static ToolBarItemDefinition StopDebuggingToolBarItem = new ToolBarItemDefinition
           <StopDebuggingCommandDefinition>(
           DebuggingGroup, 3);

        public static ToolBarItemDefinition RestartDebuggingToolBarItem = new ToolBarItemDefinition
           <RestartDebuggingCommandDefinition>(
           DebuggingGroup, 4);

        public static ToolBarItemDefinition StepOverToolBarItem = new ToolBarItemDefinition
           <StepOverCommandDefinition>(
           DebuggingGroup, 5);

        public static ToolBarItemDefinition StepIntoToolBarItem = new ToolBarItemDefinition
           <StepIntoCommandDefinition>(
           DebuggingGroup, 6);

        public static ToolBarItemDefinition StepOutToolBarItem = new ToolBarItemDefinition
           <StepOutCommandDefinition>(
           DebuggingGroup, 7);

        public void BeforeActivation()
        {
            
        }

        public void Activation()
        {
            
        }
    }
}