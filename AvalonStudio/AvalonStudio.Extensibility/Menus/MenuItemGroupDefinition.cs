using AvalonStudio.Extensibility.Plugin;
using System;
using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [PartNotDiscoverable]
	public class MenuItemGroupDefinition : IExtension
	{
		public MenuItemGroupDefinition(MenuDefinitionBase parent, int sortOrder)
		{
			Parent = parent;
			SortOrder = sortOrder;
		}

		public MenuDefinitionBase Parent { get; }

		public int SortOrder { get; }

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            
        }
    }
}