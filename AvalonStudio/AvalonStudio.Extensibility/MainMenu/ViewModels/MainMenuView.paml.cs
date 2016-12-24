using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Extensibility.MainMenu.ViewModels
{
	public class MainMenuView : UserControl
	{
		public MainMenuView()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}