namespace AvalonStudio.Debugging
{
    using Avalonia;
    using Avalonia.Controls;

    public class RegistersView : UserControl
    {
        public RegistersView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
