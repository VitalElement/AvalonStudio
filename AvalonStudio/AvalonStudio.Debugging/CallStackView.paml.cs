namespace AvalonStudio.Debugging
{
    using Avalonia;
    using Avalonia.Controls;

    public class CallStackView : UserControl
    {
        public CallStackView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
