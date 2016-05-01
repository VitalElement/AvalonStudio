namespace AvalonStudio.Extensibility.ToolBars
{
    public class ExcludeToolBarItemGroupDefinition
    {
        private readonly ToolBarItemGroupDefinition _toolBarItemGroupDefinitionToExclude;
        public ToolBarItemGroupDefinition ToolBarItemGroupDefinitionToExclude
        {
            get { return _toolBarItemGroupDefinitionToExclude; }
        }

        public ExcludeToolBarItemGroupDefinition(ToolBarItemGroupDefinition toolBarItemGroupDefinition)
        {
            _toolBarItemGroupDefinitionToExclude = toolBarItemGroupDefinition;
        }
    }
}