namespace AvalonStudio.Extensibility.Controls
{
    using Avalonia;
    using Avalonia.Markup.Xaml;
    using Avalonia.Styling;
    using System;

    public class ControlTheme : Styles
    {
        public ControlTheme()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}