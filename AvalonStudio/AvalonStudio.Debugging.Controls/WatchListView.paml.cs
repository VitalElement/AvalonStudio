using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Debugging.Controls
{
    public class WatchListView : UserControl
    {
        public WatchListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}