namespace AvalonStudio.Extensibility.Menus
{
    using AvalonStudio.Extensibility.Commands;

    public class MenuItemDefinition
    {
        public MenuItemDefinition(MenuItemGroupDefinition group, string text, int sortOrder)
        {
            Group = group;
            SortOrder = sortOrder;
            Text = text;

            IoC.RegisterConstant(this);
        }

        public virtual void Activation()
        {
        }

        public MenuItemGroupDefinition Group { get; protected set; }

        public CommandDefinition CommandDefinition { get; protected set; }

        public string Text { get; }

        public int SortOrder { get; }
    }

    public class MenuItemDefinition<TCommand> : MenuItemDefinition where TCommand : CommandDefinition
    {
        public MenuItemDefinition(MenuItemGroupDefinition group, string text, int sortOrder) : base(group, text, sortOrder)
        {
        }

        public override void Activation()
        {
            base.Activation();

            CommandDefinition = IoC.Get<TCommand>();
        }
    }
}