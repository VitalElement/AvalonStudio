using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Toolchains.LocalGCC
{
	public class ToolchainSettingsForm : UserControl
	{
		public ToolchainSettingsForm()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}