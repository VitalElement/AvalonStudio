using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Debugging.Controls
{
    public class RegistersView : UserControl
    {
        public RegistersView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}