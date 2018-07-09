using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Studio
{
    public class SearchResult : UserControl
    {
        public SearchResult()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}