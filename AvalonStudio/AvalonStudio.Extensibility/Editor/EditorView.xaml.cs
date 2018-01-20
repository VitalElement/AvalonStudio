using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Extensibility.Editor
{
    public class EditorView : UserControl
    {
        public EditorView()
        {
            InitializeComponent();
        }

        ~EditorView ()
        {
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}