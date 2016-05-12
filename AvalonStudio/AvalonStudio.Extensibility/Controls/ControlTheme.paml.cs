namespace AvalonStudio.Extensibility.Controls
{
    using Avalonia;
    using Avalonia.Styling;
    using System;

    public class ControlTheme : Styles
    {
        public ControlTheme()
        {
            this.LoadFromXaml();
        }
    }
}