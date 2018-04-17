using Avalonia.Ide.CompletionEngine;
using Avalonia.Input;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using System;

namespace AvalonStudio.Languages.Xaml
{
    class CompleteCloseTagCodeEditorHelper : ICodeEditorInputHelper
    {
        public void AfterTextInput(ILanguageService languageServivce, IEditor editor, TextInputEventArgs args)
        {
            if (args.Text == ">")
            {
                var textBefore = editor.Document.GetText(0, Math.Max(0, editor.CaretOffset - 1));

                if (textBefore.Length > 2 && textBefore[textBefore.Length - 1] != '/')
                {
                    var state = XmlParser.Parse(textBefore);
                    if (!state.TagName.StartsWith('/') && (state.State == XmlParser.ParserState.InsideElement
                        || state.State == XmlParser.ParserState.StartElement
                        || state.State == XmlParser.ParserState.AfterAttributeValue))
                    {
                        var caret = editor.CaretOffset;
                        editor.Document.Insert(caret, $"</{state.TagName}>");
                        editor.CaretOffset = caret;
                    }
                }
            }
        }

        public void BeforeTextInput(ILanguageService languageService, IEditor editor, TextInputEventArgs args)
        {
        }
    }
}
