namespace AvalonStudio.Debugging
{
    using Avalonia.Controls;
    using Avalonia;

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
