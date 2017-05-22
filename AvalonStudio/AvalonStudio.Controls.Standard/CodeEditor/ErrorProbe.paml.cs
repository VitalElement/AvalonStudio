using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class ErrorProbeView : UserControl
    {
        public ErrorProbeView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}