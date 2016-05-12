using Avalonia.Controls;
using Avalonia;

namespace AvalonStudio.Toolchains.STM32
{
    public class CompileSettingsForm : TabItem
    {
        public CompileSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
