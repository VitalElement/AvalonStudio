using AvalonStudio.Extensibility.Plugin;
using System;
using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [PartNotDiscoverable]
	public abstract class MenuItemGroupDefinition : IExtension
	{
        private Func<MenuDefinitionBase> getParent;

		public MenuItemGroupDefinition(Func<MenuDefinitionBase> parent, int sortOrder)
		{
			getParent = parent;
			SortOrder = sortOrder;
		}

		public MenuDefinitionBase Parent { get; private set; }

		public int SortOrder { get; }

        public void Activation()
        {
            Parent = getParent();
        }

        public abstract void BeforeActivation();
    }
}