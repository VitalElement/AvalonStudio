namespace AvalonStudio.Controls.Standard.AboutScreen
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    public class AboutDialogView : UserControl
    {
        public AboutDialogView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
