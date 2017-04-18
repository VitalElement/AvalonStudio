using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ReferenceSettingsFormView : UserControl
    {
        public ReferenceSettingsFormView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}