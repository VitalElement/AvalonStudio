using Perspex.Controls;
using Perspex.Markup.Xaml;

namespace AvalonStudio.Controls
{
    public class ModalDialog : Window
    {
        public ModalDialog()
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
