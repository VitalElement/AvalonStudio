using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Markup.Xaml;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.GlobalSettings;
using AvalonStudio.Shell.Controls;
using AvalonStudio.Extensibility;

namespace AvalonStudio
{
	public class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();

            AvaloniaLocator.CurrentMutable.BindToSelf<Window>(this);
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
