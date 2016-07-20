using System;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;

namespace AvalonStudio.Extensibility.Menus
{
	public class MenuDefinition : MenuDefinitionBase
	{
		public MenuDefinition(MenuBarDefinition menuBar, int sortOrder, string text)
		{
			MenuBar = menuBar;
			SortOrder = sortOrder;
			Text = text;
		}

		public MenuBarDefinition MenuBar { get; }

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
	}
}