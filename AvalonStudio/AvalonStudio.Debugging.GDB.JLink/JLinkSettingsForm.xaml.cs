using Perspex.Controls;
using Perspex.Markup.Xaml;

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
            PerspexXamlLoader.Load(this);
        }
    }
}
