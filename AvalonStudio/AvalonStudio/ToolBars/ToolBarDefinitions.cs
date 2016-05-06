namespace AvalonStudio.ToolBars
{
    using Extensibility.ToolBars;
    using System.ComponentModel.Composition;

    internal static class ToolBarDefinitions
    {
        [Export]
        public static ToolBarDefinition MainToolBar = new ToolBarDefinition(0, "Main");
    }
}