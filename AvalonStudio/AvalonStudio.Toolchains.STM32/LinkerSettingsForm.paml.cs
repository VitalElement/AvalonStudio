using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Toolchains.STM32
{
	public class LinkerSettingsFormView : UserControl
	{
		public LinkerSettingsFormView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}