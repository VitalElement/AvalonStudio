namespace AvalonStudio.Debugging.GDB.OpenOCD
{
    using Avalonia;
    using Avalonia.Controls;

    public class OpenOCDSettingsForm : UserControl
    {
        public OpenOCDSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
