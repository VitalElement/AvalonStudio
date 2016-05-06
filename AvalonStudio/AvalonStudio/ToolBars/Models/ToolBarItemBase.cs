using Caliburn.Micro;

namespace AvalonStudio.ToolBars.Models
{
	public class ToolBarItemBase : PropertyChangedBase
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