using System;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Extensibility.Plugin;
using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [PartNotDiscoverable]
    public abstract class MenuDefinition : MenuDefinitionBase, IExtension
	{
        private Func<MenuBarDefinition> _getMenuBar;

		public MenuDefinition(Func<MenuBarDefinition> menuBar, int sortOrder, string text)
		{
            _getMenuBar = menuBar;
            SortOrder = sortOrder;
            Text = text;
		}

		public MenuBarDefinition MenuBar { get; private set; }

		public override int SortOrder { get; }

		public override string Text { get; }

		public override Uri IconSource
		{
			get { return null; }
		}

		public override KeyGesture KeyGesture
		{
			get { return null; }
		}

		public override CommandDefinitionBase CommandDefinition
		{
			get { return null; }
		}

        public void Activation()
        {
            MenuBar = _getMenuBar();
        }

        public abstract void BeforeActivation();
    }
}