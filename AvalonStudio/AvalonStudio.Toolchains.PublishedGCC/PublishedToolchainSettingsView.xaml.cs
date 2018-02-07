using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Toolchains.PublishedGCC
{
    public class PublishedToolchainSettingsView : UserControl
    {
        public PublishedToolchainSettingsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}