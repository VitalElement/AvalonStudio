using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Extensibility.Controls
{
	public class ControlTheme : Styles
	{
        static ControlTheme()
        {
            Application.Current.Styles.Add(new ControlTheme());
        }

		public ControlTheme()
		{
            
		}

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}