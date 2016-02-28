using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Toolchains.STM32
{
    public class ToolchainSettingsForm : UserControl
    {
        public ToolchainSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
