namespace AvalonStudio.Extensibility.Menus
{
	public class ExcludeMenuItemGroupDefinition
	{
		public ExcludeMenuItemGroupDefinition(MenuItemGroupDefinition menuItemGroupDefinition)
		{
			MenuItemGroupDefinitionToExclude = menuItemGroupDefinition;
		}

		public MenuItemGroupDefinition MenuItemGroupDefinitionToExclude { get; }
	}
}