using System;
using System.Windows.Input;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Extensibility.MainMenu.Models
{
	public class TextMenuItem : StandardMenuItem
	{
		private readonly MenuDefinitionBase _menuDefinition;

		public TextMenuItem(MenuDefinitionBase menuDefinition)
		{
			_menuDefinition = menuDefinition;
		}

		public override string Text
		{
			get { return _menuDefinition.Text; }
		}

		public override Uri IconSource
		{
			get { return _menuDefinition.IconSource; }
		}

		public override string InputGestureText
		{
			get
			{
				return _menuDefinition.KeyGesture == null
					? string.Empty
					: "geststring"; // _menuDefinition.KeyGesture.GetDisplayStringForCulture(CultureInfo.CurrentUICulture);
			}
		}

		public override ICommand Command
		{
			get { return null; }
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