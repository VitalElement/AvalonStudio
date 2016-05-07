namespace AvalonStudio.Debugging
{
    using Perspex;
    using Perspex.Controls;

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
