namespace AvalonStudio.Controls
{
    using Avalonia.Controls;
    using Avalonia;

    public class ModalDialog : UserControl
    {
        public ModalDialog()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
