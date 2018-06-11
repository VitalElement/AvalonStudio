using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ShellExampleApp
{
    public class AboutDialogView : UserControl
    {
        public AboutDialogView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}