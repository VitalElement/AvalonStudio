namespace AvalonStudio.Extensibility.ToolBars
{
	public class ToolBarItemGroupDefinition
	{
		public ToolBarItemGroupDefinition(ToolBarDefinition toolBar, int sortOrder)
		{
            IoC.RegisterConstant(this);
			ToolBar = toolBar;
			SortOrder = sortOrder;
		}

		public ToolBarDefinition ToolBar { get; }

		public int SortOrder { get; }
	}
}