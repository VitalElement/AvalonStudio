namespace AvalonStudio.Extensibility.Menus
{
    using AvalonStudio.Extensibility.Commands;

    public class MenuItemDefinition
    {
        public MenuItemDefinition(MenuItemGroupDefinition group, int sortOrder)
        {
            Group = group;
            SortOrder = sortOrder;

            IoC.RegisterConstant(this);
        }

        public virtual void Activation()
        {
        }

        public MenuItemGroupDefinition Group { get; protected set; }

        public CommandDefinition CommandDefinition { get; protected set; }

        public string Text { get; protected set; }

        public int SortOrder { get; }
    }
}