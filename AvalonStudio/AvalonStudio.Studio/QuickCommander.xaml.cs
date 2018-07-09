using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Studio
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