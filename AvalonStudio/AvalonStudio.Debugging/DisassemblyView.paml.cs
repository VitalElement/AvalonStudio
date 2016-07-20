using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Debugging
{
	public class DisassemblyView : UserControl
	{
		public DisassemblyView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}