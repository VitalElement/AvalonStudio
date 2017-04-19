namespace AvalonStudio.Extensibility.Menus
{
    using AvalonStudio.Extensibility.Commands;

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
