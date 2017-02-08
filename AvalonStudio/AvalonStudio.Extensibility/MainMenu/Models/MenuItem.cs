using System;
using System.Windows.Input;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Extensibility.MainMenu.Models
{
	public class MenuItem : StandardMenuItem
	{
		private readonly MenuItemDefinition _menuDefinition;

		public MenuItem(MenuItemDefinition menuDefinition)
		{
			_menuDefinition = menuDefinition;
		}

		public override string Text
		{
			get { return _menuDefinition.Text; }
		}

		public override ICommand Command
		{
			get { return _menuDefinition.CommandDefinition?.Command; }
		}

		public override bool IsChecked
		{
			get { return false; }
		}

		public override bool IsVisible
		{
			get { return true; }
		}
    }
}