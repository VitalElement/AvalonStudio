namespace AvalonStudio.Controls
{
    using Avalonia.Controls;
    using Avalonia;

    public class DebugHoverProbe : UserControl
    {
        public DebugHoverProbe()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
