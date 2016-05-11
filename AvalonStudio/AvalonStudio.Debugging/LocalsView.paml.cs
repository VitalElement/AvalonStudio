namespace AvalonStudio.Debugging
{
    using Perspex.Controls;
    using Perspex;

    public class LocalsView : UserControl
    {
        public LocalsView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
