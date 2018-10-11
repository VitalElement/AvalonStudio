using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Markup.Xaml;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.GlobalSettings;
using AvalonStudio.Shell.Controls;

namespace AvalonStudio
{
	public class MainWindow : MetroWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			this.AttachDevTools();

			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
 			{
 				HasSystemDecorations = true;

 				// This will need implementing properly once this is supported by avalonia itself.
 				var color = (ColorTheme.CurrentTheme.Background as SolidColorBrush).Color;
 				(PlatformImpl as Avalonia.Native.WindowImpl).SetTitleBarColor(color);
 			}
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}
