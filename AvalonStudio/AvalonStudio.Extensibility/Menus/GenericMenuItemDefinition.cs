namespace AvalonStudio.Extensibility.Menus
{
    using AvalonStudio.Extensibility.Commands;

    public class MenuItemDefinition<TCommand> : MenuItemDefinition where TCommand : CommandDefinition
    {
        public MenuItemDefinition(MenuItemGroupDefinition group, int sortOrder) : base(group, sortOrder)
        {
        }

        public override void Activation()
        {
            base.Activation();

            CommandDefinition = IoC.Get<TCommand>();

            Text = CommandDefinition.Text;
        }
    }
}
