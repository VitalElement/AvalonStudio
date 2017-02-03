using System.ComponentModel.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [Export(typeof(MenuItemGroupDefinition))]
	public class MenuItemGroupDefinition
	{
		public MenuItemGroupDefinition(MenuDefinitionBase parent, int sortOrder)
		{
			Parent = parent;
			SortOrder = sortOrder;
		}

		public MenuDefinitionBase Parent { get; }

		public int SortOrder { get; }
	}
}