using System;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [PartNotDiscoverable]
    public class TextMenuItemDefinition : MenuItemDefinition
	{
		public TextMenuItemDefinition(Func<MenuItemGroupDefinition> group, int sortOrder, string text, Uri iconSource = null)
			: base(group, sortOrder)
		{
			Text = text;
			IconSource = iconSource;
		}

		public override string Text { get; }

		public override Uri IconSource { get; }

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