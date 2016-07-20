namespace AvalonStudio.Extensibility.Menus
{
	public class ExcludeMenuDefinition
	{
		public ExcludeMenuDefinition(MenuDefinition menuDefinition)
		{
			MenuDefinitionToExclude = menuDefinition;
		}

		public MenuDefinition MenuDefinitionToExclude { get; }
	}
}