using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaEdit.Highlighting;

namespace AvalonStudio.Languages.Xaml
{
    public class XamlEditorView : UserControl
    {
        public XamlEditorView()
        {
            InitializeComponent();
            var editor = this.FindControl<AvaloniaEdit.TextEditor>("editor");
            editor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("XML");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}