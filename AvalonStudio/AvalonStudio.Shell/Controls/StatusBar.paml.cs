using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Shell.Controls
{
    public class StatusBar : UserControl
    {
        public StatusBar()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}