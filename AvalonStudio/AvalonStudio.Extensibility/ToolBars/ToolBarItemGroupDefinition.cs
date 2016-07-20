namespace AvalonStudio.Extensibility.ToolBars
{
	public class ToolBarItemGroupDefinition
	{
		public ToolBarDefinition ToolBar { get; }

		public int SortOrder { get; }

		public ToolBarItemGroupDefinition(ToolBarDefinition toolBar, int sortOrder)
		{
			ToolBar = toolBar;
			SortOrder = sortOrder;
		}
	}
}