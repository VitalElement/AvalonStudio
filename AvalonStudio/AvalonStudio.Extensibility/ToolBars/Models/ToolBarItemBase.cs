namespace AvalonStudio.Extensibility.ToolBars.Models
{
	using ReactiveUI;

	public class ToolBarItemBase : ReactiveObject
	{
		public static ToolBarItemBase Separator => new ToolBarItemSeparator();

		public virtual string Name => "-";
	}
}