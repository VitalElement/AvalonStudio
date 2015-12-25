namespace AvalonStudio.TextEditor
{
    using Perspex.Markup.Xaml;
    using Perspex.Styling;

    public class TextEditorTheme : Styles
    {
        public TextEditorTheme()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
