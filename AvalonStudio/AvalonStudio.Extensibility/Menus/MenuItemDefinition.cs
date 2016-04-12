namespace AvalonStudio.Extensibility.Menus
{
    public abstract class MenuItemDefinition : MenuDefinitionBase
    {
        private readonly MenuItemGroupDefinition _group;
        private readonly int _sortOrder;

        public MenuItemGroupDefinition Group
        {
            get { return _group; }
        }

        public override int SortOrder
        {
            get { return _sortOrder; }
        }

        protected MenuItemDefinition(MenuItemGroupDefinition group, int sortOrder)
        {
            _group = group;
            _sortOrder = sortOrder;
        }
    }
}