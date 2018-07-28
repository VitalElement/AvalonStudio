using Avalonia.Ide.CompletionEngine;
using Avalonia.Input;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using System;

namespace AvalonStudio.Languages.Xaml
{
    class InsertQuotesForPropertyValueCodeEditorHelper : ITextEditorInputHelper
    {
        public bool AfterTextInput(ITextEditor editor, string text)
        {
            if (text == "=")
            {
                var textBefore = editor.Document.GetText(0, Math.Max(0, editor.Offset - 1));

                if (textBefore.Length > 2 && textBefore[textBefore.Length - 1] != '/')
                {
                    var state = XmlParser.Parse(textBefore);
                    if (state.State == XmlParser.ParserState.StartAttribute)
                    {
                        var caret = editor.Offset;
                        editor.Document.Insert(caret, "\"\"");
                        editor.Offset = caret + 1;
                    }
                }
            }

            return false;
        }

        public bool BeforeTextInput(ITextEditor editor, string text)
        {
            return false;
        }

        public void CaretMovedToEmptyLine(ITextEditor editor)
        {
        }
    }
}
