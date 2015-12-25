namespace AvalonStudio.TextEditor
{
    using Perspex.Markup.Xaml;
    using Perspex.Styling;
    using System;

    public class TextEditorTheme : Styles
    {
        public TextEditorTheme()
        {
            var res = System.Reflection.Assembly.GetAssembly(typeof(TextEditorTheme)).GetManifestResourceNames();

            foreach (var resource in res)
            {
                Console.WriteLine(resource);
            }

            PerspexXamlLoader.Load(this);
        }
    }
}
