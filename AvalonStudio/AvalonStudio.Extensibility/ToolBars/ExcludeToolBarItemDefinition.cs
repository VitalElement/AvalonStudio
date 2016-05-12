namespace AvalonStudio.Extensibility.ToolBars
{
    public class ExcludeToolBarItemDefinition
    {
        private readonly ToolBarItemDefinition _toolBarItemDefinitionToExclude;
        public ToolBarItemDefinition ToolBarItemDefinitionToExclude
        {
            get { return _toolBarItemDefinitionToExclude; }
        }

        public ExcludeToolBarItemDefinition(ToolBarItemDefinition ToolBarItemDefinition)
        {
            _toolBarItemDefinitionToExclude = ToolBarItemDefinition;
        }
    }
}