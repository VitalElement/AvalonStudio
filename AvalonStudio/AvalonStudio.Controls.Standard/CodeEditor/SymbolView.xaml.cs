using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class SymbolView : UserControl
    {
        public SymbolView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}