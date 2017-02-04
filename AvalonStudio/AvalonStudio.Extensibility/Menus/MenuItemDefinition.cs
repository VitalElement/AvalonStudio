using AvalonStudio.Extensibility.Plugin;
using System;
using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [PartNotDiscoverable]
    public abstract class MenuItemDefinition : MenuDefinitionBase, IExtension
	{
        private Func<MenuItemGroupDefinition> _getGroup;

		protected MenuItemDefinition(Func<MenuItemGroupDefinition> group, int sortOrder)
		{
			_getGroup = group;
			SortOrder = sortOrder;
		}

		public MenuItemGroupDefinition Group { get; private set; }

		public override int SortOrder { get; }

        public void Activation()
        {
            Group = _getGroup();
        }

        public void BeforeActivation()
        {
            
        }
    }
}