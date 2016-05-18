namespace AvalonStudio.TextEditor
{
    using Avalonia;
    using Avalonia.Markup.Xaml;
    using Avalonia.Styling;

    public class TextEditorTheme : Styles
    {
        public TextEditorTheme()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
