namespace AvalonStudio.Extensibility.Menus
{
    public class ExcludeMenuDefinition
    {
        private readonly MenuDefinition _menuDefinitionToExclude;
        public MenuDefinition MenuDefinitionToExclude 
        { 
            get { return _menuDefinitionToExclude; } 
        }

        public ExcludeMenuDefinition(MenuDefinition menuDefinition)
        {
            _menuDefinitionToExclude = menuDefinition;
        }
    }
}
