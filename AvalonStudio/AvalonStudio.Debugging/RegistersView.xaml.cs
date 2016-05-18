namespace AvalonStudio.Debugging
{
    using Avalonia;
    using Avalonia.Controls;

    public class RegistersView : UserControl
    {
        public RegistersView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
