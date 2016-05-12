using Avalonia.Controls;
using Avalonia;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ComponentSettingsForm : TabItem
    {
        public ComponentSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
