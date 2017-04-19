namespace AvalonStudio.Extensibility.ToolBars
{
    public class ExcludeToolBarItemGroupDefinition
    {
        public ExcludeToolBarItemGroupDefinition(ToolBarItemGroupDefinition toolBarItemGroupDefinition)
        {
            ToolBarItemGroupDefinitionToExclude = toolBarItemGroupDefinition;
        }

        public ToolBarItemGroupDefinition ToolBarItemGroupDefinitionToExclude { get; }
    }
}