namespace AvalonStudio.Extensibility.Menus
{
    public class ExcludeMenuItemDefinition
    {
        public ExcludeMenuItemDefinition(MenuItemDefinition menuItemDefinition)
        {
            MenuItemDefinitionToExclude = menuItemDefinition;
        }

        public MenuItemDefinition MenuItemDefinitionToExclude { get; }
    }
}