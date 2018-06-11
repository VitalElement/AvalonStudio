using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio
{
    public class GeneralSettingsView : UserControl
    {
        public GeneralSettingsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}