using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls
{
	public class PackageManagerDialogView : UserControl
	{
		public PackageManagerDialogView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}