using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Extensibility.Controls
{
	public class ControlTheme : Styles, IExtension
	{
        static ControlTheme()
        {
            Application.Current.Styles.Add(new ControlTheme());
        }

		public ControlTheme()
		{
			AvaloniaXamlLoader.Load(this);
		}

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}