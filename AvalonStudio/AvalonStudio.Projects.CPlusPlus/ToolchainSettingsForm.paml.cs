using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ToolchainSettingsForm : TabItem
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
