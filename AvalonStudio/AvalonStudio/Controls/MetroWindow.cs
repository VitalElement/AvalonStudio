namespace AvalonStudio.Controls
{
    using Perspex.Controls;
    using Perspex.Styling;
    using System;

    public class MetroWindow : Window, IStyleable
    {
        public MetroWindow()
        {
        }

        Type IStyleable.StyleKey => typeof(MetroWindow);
    }
}
