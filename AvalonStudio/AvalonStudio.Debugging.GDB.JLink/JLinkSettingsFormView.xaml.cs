using Avalonia.Controls;
using Avalonia;

namespace AvalonStudio.Debugging.GDB.JLink
{
    public class JLinkSettingsFormView : UserControl
    {
        public JLinkSettingsFormView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
