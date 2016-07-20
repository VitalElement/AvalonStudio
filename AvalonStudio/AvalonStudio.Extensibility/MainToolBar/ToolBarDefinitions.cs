using System.ComponentModel.Composition;
using AvalonStudio.Extensibility.ToolBars;

namespace AvalonStudio.Extensibility.MainToolBar
{
	public static class ToolBarDefinitions
	{
		[Export] public static ToolBarDefinition MainToolBar = new ToolBarDefinition(0, "Main");
	}
}