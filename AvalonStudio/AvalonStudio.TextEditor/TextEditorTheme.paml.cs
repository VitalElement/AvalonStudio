using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace AvalonStudio.TextEditor
{
	public class TextEditorTheme : Styles
    {
		public TextEditorTheme()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}