using System;
using Avalonia.Controls;
using Avalonia.Styling;

namespace AvalonStudio.Controls
{
	public class SplashScreen : Window, IStyleable
	{
		Type IStyleable.StyleKey => typeof(SplashScreen);

	}
}
