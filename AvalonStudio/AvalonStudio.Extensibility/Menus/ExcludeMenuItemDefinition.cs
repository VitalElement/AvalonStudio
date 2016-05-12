namespace AvalonStudio.Extensibility.Menus
{
    public class ExcludeMenuItemDefinition
    {
        private readonly MenuItemDefinition _menuItemDefinitionToExclude;
        public MenuItemDefinition MenuItemDefinitionToExclude 
        { 
            get { return _menuItemDefinitionToExclude; } 
        }

        public ExcludeMenuItemDefinition(MenuItemDefinition menuItemDefinition)
        {
            _menuItemDefinitionToExclude = menuItemDefinition;
        }
    }
}
