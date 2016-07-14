using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ClassTemplateSettingsView : UserControl
    {
        public ClassTemplateSettingsView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}