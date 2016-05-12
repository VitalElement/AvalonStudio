namespace AvalonStudio.Debugging
{
    using Avalonia.Controls;
    using Avalonia;

    public class WatchListView : UserControl
    {
        public WatchListView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
