using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Shell.Controls
{
    public class QuickCommander : UserControl
    {
        public QuickCommander()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}