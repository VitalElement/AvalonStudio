using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace AvalonStudio.Extensibility.Controls
{
	public class ControlTheme : Styles
	{
		public ControlTheme()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}