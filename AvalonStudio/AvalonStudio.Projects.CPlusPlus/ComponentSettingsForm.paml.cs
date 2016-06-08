using Avalonia.Controls;
using Avalonia;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ComponentSettingsForm : UserControl
    {
        public ComponentSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
