namespace AvalonStudio.Extensibility.ToolBars
{
	using System;
	using Avalonia.Input;
	using Commands;

	public abstract class ToolBarItemDefinition
	{
		public ToolBarItemGroupDefinition Group { get; }

		public int SortOrder { get; }

		public ToolBarItemDisplay Display { get; }

		public abstract string Text { get; }
		public abstract Uri IconSource { get; }
		public abstract KeyGesture KeyGesture { get; }
		public abstract CommandDefinitionBase CommandDefinition { get; }

		protected ToolBarItemDefinition(ToolBarItemGroupDefinition group, int sortOrder, ToolBarItemDisplay display)
		{
			Group = group;
			SortOrder = sortOrder;
			Display = display;
		}
	}

	public enum ToolBarItemDisplay
	{
		IconOnly,
		IconAndText
	}
}