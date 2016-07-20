using ReactiveUI;

namespace AvalonStudio.Extensibility.ToolBars.Models
{
	public class ToolBarItemBase : ReactiveObject
	{
		public static ToolBarItemBase Separator => new ToolBarItemSeparator();

		public virtual string Name => "-";
	}
}