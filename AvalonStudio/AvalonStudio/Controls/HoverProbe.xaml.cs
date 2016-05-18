namespace AvalonStudio.Controls
{
    using Avalonia.Controls;
    using Avalonia;

    public class HoverProbe : UserControl
    {
        public HoverProbe()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
