namespace AvalonStudio.Extensibility.ToolBars.Models
{
    using ReactiveUI;

    public class ToolBarItemBase : ReactiveObject
	{
		public static ToolBarItemBase Separator
		{
			get { return new ToolBarItemSeparator(); }
		}

		public virtual string Name
		{
			get { return "-"; }
		}
	}
}