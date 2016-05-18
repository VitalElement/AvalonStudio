namespace AvalonStudio.Controls
{
    using Avalonia;
    using Avalonia.Controls;

    public class StatusBar : UserControl
    {
        public StatusBar()
        {
            this.InitializeComponent();            
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
