namespace AvalonStudio.Extensibility.Menus
{
	public abstract class MenuItemDefinition : MenuDefinitionBase
	{
		protected MenuItemDefinition(MenuItemGroupDefinition group, int sortOrder)
		{
			Group = group;
			SortOrder = sortOrder;
		}

		public MenuItemGroupDefinition Group { get; }

		public override int SortOrder { get; }
	}
}