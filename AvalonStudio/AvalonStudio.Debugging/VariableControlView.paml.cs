namespace AvalonStudio.Debugging
{
    using Perspex;
    using Perspex.Controls;

    public class VariableControlView : UserControl
    {
        public VariableControlView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
