namespace AvalonStudio.Debugging.GDB.OpenOCD
{
    using Avalonia;
    using Avalonia.Controls;

    public class OpenOCDSettingsFormView : UserControl
    {
        public OpenOCDSettingsFormView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
