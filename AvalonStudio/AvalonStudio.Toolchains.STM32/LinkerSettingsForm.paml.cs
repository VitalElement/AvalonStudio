using Avalonia.Controls;
using Avalonia;

namespace AvalonStudio.Toolchains.STM32
{
    public class LinkerSettingsForm : TabItem
    {
        public LinkerSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
