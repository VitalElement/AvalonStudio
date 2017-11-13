using Avalonia.Ide.CompletionEngine;
using Avalonia.Input;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Documents;
using System;

namespace AvalonStudio.Languages.Xaml
{
    class InsertQuotesForPropertyValueCodeEditorHelper : ICodeEditorInputHelper
    {
        public void AfterTextInput(ILanguageService languageServivce, ITextDocument document, TextInputEventArgs args)
        {
            if (args.Text == "=")
            {
                var textBefore = document.Text.Substring(0, Math.Max(0, document.Caret - 1));
                if (textBefore.Length > 2 && textBefore[textBefore.Length - 1] != '/')
                {
                    var state = XmlParser.Parse(textBefore);
                    if (state.State == XmlParser.ParserState.StartAttribute)
                    {
                        var caret = document.Caret;
                        document.Replace(caret, 0, "\"\" ");
                        document.Caret = caret + 1;
                    }
                }
            }
        }

        public void BeforeTextInput(ILanguageService languageService, ITextDocument document, TextInputEventArgs args)
        {
        }
    }
}
