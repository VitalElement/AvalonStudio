using Avalonia.Controls;
using Avalonia;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class DebuggerSettingsForm : TabItem
    {
        public DebuggerSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
