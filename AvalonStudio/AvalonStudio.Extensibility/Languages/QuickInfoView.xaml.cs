using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Extensibility.Languages
{
    public class QuickInfoView : UserControl
    {
        public QuickInfoView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}