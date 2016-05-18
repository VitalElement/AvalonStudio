namespace AvalonStudio.Controls
{
    using Avalonia;
    using Avalonia.Controls;

    public class ToolBar : UserControl
    {
        public ToolBar()
        {
            this.InitializeComponent();            
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
