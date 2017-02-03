using System.Composition;

namespace AvalonStudio.Extensibility.ToolBars
{
    //[InheritedExport]
	public class ToolBarItemGroupDefinition
	{
		public ToolBarItemGroupDefinition(ToolBarDefinition toolBar, int sortOrder)
		{
			ToolBar = toolBar;
			SortOrder = sortOrder;
		}

		public ToolBarDefinition ToolBar { get; }

		public int SortOrder { get; }
	}
}