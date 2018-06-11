using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Shell.Controls
{
    public class ModalDialog : MetroWindow
    {
        public ModalDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}