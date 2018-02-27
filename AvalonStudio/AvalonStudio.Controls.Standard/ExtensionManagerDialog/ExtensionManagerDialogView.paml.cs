using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls.Standard.ExtensionManagerDialog
{
    public class ExtensionManagerDialogView : UserControl
    {
        public ExtensionManagerDialogView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
