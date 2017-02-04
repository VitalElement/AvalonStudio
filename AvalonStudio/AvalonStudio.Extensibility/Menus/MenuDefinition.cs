using System;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Extensibility.Plugin;
using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [PartNotDiscoverable]
    public abstract class MenuDefinition : IExtension
	{
        private Func<MenuBarDefinition> _getMenuBar;
        private Func<CommandDefinition> _getCommandDefinition;

		public MenuDefinition(Func<MenuBarDefinition> menuBar, int sortOrder, string text)
		{
            _getMenuBar = menuBar;
            SortOrder = sortOrder;
            Text = text;
		}

		public MenuBarDefinition MenuBar { get; private set; }

		public int SortOrder { get; }

		public string Text { get; }
        

        public void Activation()
        {
            MenuBar = _getMenuBar();
        }

        public abstract void BeforeActivation();
    }
}