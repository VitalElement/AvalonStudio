namespace AvalonStudio.Extensibility.ToolBars
{
    public class ExcludeToolBarDefinition
    {
        private readonly ToolBarDefinition _toolBarDefinitionToExclude;
        public ToolBarDefinition ToolBarDefinitionToExclude
        {
            get { return _toolBarDefinitionToExclude; }
        }

        public ExcludeToolBarDefinition(ToolBarDefinition toolBarDefinition)
        {
            _toolBarDefinitionToExclude = toolBarDefinition;
        }
    }
}