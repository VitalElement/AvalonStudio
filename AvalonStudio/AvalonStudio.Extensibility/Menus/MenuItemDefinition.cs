using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [Export(typeof(MenuItemDefinition))]
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