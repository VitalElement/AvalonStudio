using Avalonia.Controls;
using Avalonia;

namespace AvalonStudio.Controls
{
    public class PackageManagerDialogView : UserControl
    {
        public PackageManagerDialogView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
