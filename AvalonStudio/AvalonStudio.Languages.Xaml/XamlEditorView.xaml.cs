using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Languages.Xaml
{
    public class XamlEditorView : UserControl
    {
        public XamlEditorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}