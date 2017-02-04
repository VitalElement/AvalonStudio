using AvalonStudio.Extensibility.Plugin;
using System;
using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [PartNotDiscoverable]
	public abstract class MenuItemGroupDefinition : IExtension
	{
        private Func<MenuDefinition> getParent;

		public MenuItemGroupDefinition(Func<MenuDefinition> parent, int sortOrder)
		{
			getParent = parent;
			SortOrder = sortOrder;
		}

		public MenuDefinition Parent { get; private set; }

		public int SortOrder { get; }

        public void Activation()
        {
            Parent = getParent();
        }

        public abstract void BeforeActivation();
    }
}