namespace AvalonStudio.Extensibility.ToolBars
{
    public class ToolBarItemGroupDefinition
    {
        private readonly ToolBarDefinition _toolBar;
        private readonly int _sortOrder;

        public ToolBarDefinition ToolBar
        {
            get { return _toolBar; }
        }

        public int SortOrder
        {
            get { return _sortOrder; }
        }

        public ToolBarItemGroupDefinition(ToolBarDefinition toolBar, int sortOrder)
        {
            _toolBar = toolBar;
            _sortOrder = sortOrder;
        }
    }
}