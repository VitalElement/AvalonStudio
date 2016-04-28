namespace AvalonStudio.Extensibility.Menus
{
    public class MenuItemGroupDefinition
    {
        private readonly MenuDefinitionBase _parent;
        private readonly int _sortOrder;

        public MenuDefinitionBase Parent
        {
            get { return _parent; }
        }

        public int SortOrder
        {
            get { return _sortOrder; }
        }

        public MenuItemGroupDefinition(MenuDefinitionBase parent, int sortOrder)
        {
            _parent = parent;
            _sortOrder = sortOrder;
        }
    }
}