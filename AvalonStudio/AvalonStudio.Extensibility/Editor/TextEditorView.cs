using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Extensibility.Editor
{
    public class TextEditorView : UserControl
    {
        public TextEditorView()
        {
            InitializeComponent();
        }

        ~TextEditorView ()
        {
            System.Console.WriteLine("TextEditorView disposed");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}