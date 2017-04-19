namespace AvalonStudio.Extensibility.Menus
{
    public class MenuItemGroupDefinition
    {
        public MenuItemGroupDefinition(MenuDefinition parent, int sortOrder)
        {
            Parent = parent;
            SortOrder = sortOrder;

            IoC.RegisterConstant(this);
        }

        public MenuDefinition Parent { get; protected set; }

        public int SortOrder { get; }

        public void Activation()
        {
        }
    }
}