namespace AvalonStudio.Debugging
{
    using Avalonia.Controls;
    using Avalonia;

    public class LocalsView : UserControl
    {
        public LocalsView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
