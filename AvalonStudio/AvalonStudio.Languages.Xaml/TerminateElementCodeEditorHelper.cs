using Avalonia.Ide.CompletionEngine;
using Avalonia.Input;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Languages.Xaml
{
    class TerminateElementCodeEditorHelper : ICodeEditorInputHelper
    {
        public void AfterTextInput(ILanguageService languageServivce, IEditor editor, TextInputEventArgs args)
        {
            if (args.Text == "/")
            {
                var textBefore = editor.Document.Text.Substring(0, Math.Max(0, editor.Offset - 1));
                if (textBefore.Length > 2 && textBefore[textBefore.Length - 1] != '/')
                {
                    var state = XmlParser.Parse(textBefore);
                    if (state.State == XmlParser.ParserState.InsideElement
                        || state.State == XmlParser.ParserState.StartElement
                        || state.State == XmlParser.ParserState.AfterAttributeValue)
                    {
                        var caret = editor.Offset;
                        editor.Document.Replace(caret, 0, $">");
                        editor.Offset = caret + 1;
                    }
                }
            }
        }

        public void BeforeTextInput(ILanguageService languageService, IEditor editor, TextInputEventArgs args)
        {
        }
    }
}
