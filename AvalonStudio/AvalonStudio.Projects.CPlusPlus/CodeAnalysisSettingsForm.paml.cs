using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class CodeAnalysisSettingsFormView : UserControl
    {
        public CodeAnalysisSettingsFormView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}