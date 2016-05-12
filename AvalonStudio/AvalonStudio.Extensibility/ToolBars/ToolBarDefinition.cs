namespace AvalonStudio.Extensibility.ToolBars
{
    public class ToolBarDefinition
    {
        private readonly int _sortOrder;
        private readonly string _name;

        public int SortOrder
        {
            get { return _sortOrder; }
        }

        public string Name
        {
            get { return _name; }
        }

        public ToolBarDefinition(int sortOrder, string name)
        {
            _sortOrder = sortOrder;
            _name = name;
        }
    }
}