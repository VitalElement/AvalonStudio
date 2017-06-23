using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class IntellisenseUserControl : UserControl
    {
        public IntellisenseUserControl()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}