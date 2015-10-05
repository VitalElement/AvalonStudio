using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class PackageManagerDialog : Window
    {
        public PackageManagerDialog()
        {
            this.InitializeComponent();
            App.AttachDevTools(this);
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
