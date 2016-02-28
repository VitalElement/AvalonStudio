using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Debugging.GDB.OpenOCD
{
    public class OpenOCDSettingsForm : UserControl
    {
        public OpenOCDSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
