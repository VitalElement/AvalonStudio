using Avalonia.Ide.CompletionEngine;
using Avalonia.Input;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Languages.Xaml
{
    class CompleteCloseTagCodeEditorHelper : ICodeEditorInputHelper
    {
        public void AfterTextInput(ILanguageService languageServivce, ITextDocument document, TextInputEventArgs args)
        {
            if (args.Text == ">")
            {
                var textBefore = document.Text.Substring(0, Math.Max(0, document.Caret - 1));
                if (textBefore.Length > 2 && textBefore[textBefore.Length - 1] != '/')
                {
                    var state = XmlParser.Parse(textBefore);
                    if (state.State == XmlParser.ParserState.InsideElement
                        || state.State == XmlParser.ParserState.StartElement
                        || state.State == XmlParser.ParserState.AfterAttributeValue)
                    {
                        var caret = document.Caret;
                        document.Replace(caret, 0, $"</{state.TagName}>");
                        document.Caret = caret;
                    }
                }
            }
        }

        public void BeforeTextInput(ILanguageService languageService, ITextDocument document, TextInputEventArgs args)
        {
        }
    }
}
