using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Debugging.Controls
{
    public class CallStackView : UserControl
    {
        public CallStackView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}