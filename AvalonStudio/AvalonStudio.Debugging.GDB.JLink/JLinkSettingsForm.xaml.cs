using Avalonia.Controls;
using Avalonia;

namespace AvalonStudio.Debugging.GDB.JLink
{
    public class JLinkSettingsForm : UserControl
    {
        public JLinkSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
