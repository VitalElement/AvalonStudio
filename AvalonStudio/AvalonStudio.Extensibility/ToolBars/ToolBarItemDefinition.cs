using System;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using System.Composition;

namespace AvalonStudio.Extensibility.ToolBars
{
    ////[InheritedExport]
	public abstract class ToolBarItemDefinition
	{
		protected ToolBarItemDefinition(ToolBarItemGroupDefinition group, int sortOrder, ToolBarItemDisplay display)
		{
			Group = group;
			SortOrder = sortOrder;
			Display = display;
		}

		public ToolBarItemGroupDefinition Group { get; }

		public int SortOrder { get; }

		public ToolBarItemDisplay Display { get; }

		public abstract string Text { get; }
		public abstract Uri IconSource { get; }
		public abstract KeyGesture KeyGesture { get; }
		public abstract CommandDefinitionBase CommandDefinition { get; }
	}

	public enum ToolBarItemDisplay
	{
		IconOnly,
		IconAndText
	}
}