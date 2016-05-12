namespace AvalonStudio.Toolchains.LocalGCC
{
    using Avalonia;
    using Avalonia.Controls;

    public class ToolchainSettingsForm : UserControl
    {
        public ToolchainSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
