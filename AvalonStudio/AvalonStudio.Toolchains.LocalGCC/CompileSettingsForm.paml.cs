using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Toolchains.LocalGCC
{
	public class CompileSettingsFormView : UserControl
	{
		public CompileSettingsFormView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}