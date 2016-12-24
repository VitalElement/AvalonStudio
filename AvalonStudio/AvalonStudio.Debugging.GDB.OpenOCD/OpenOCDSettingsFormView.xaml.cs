using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Debugging.GDB.OpenOCD
{
	public class OpenOCDSettingsFormView : UserControl
	{
		public OpenOCDSettingsFormView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}