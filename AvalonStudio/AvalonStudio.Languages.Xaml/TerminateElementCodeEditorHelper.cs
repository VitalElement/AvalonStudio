using Avalonia.Ide.CompletionEngine;
using Avalonia.Input;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using System;

namespace AvalonStudio.Languages.Xaml
{
    class TerminateElementCodeEditorHelper : ITextEditorInputHelper
    {
        public bool AfterTextInput(ITextEditor editor, string text)
        {
            if (text == "/")
            {
                var textBefore = editor.Document.GetText(0, Math.Max(0, editor.Offset));

                var nextChar = '\0';

                if(editor.Offset < editor.Document.TextLength)
                {
                    nextChar = editor.Document.GetCharAt(editor.Offset);
                }

                if (textBefore.Length > 2 && nextChar != '>')
                {
                    var state = XmlParser.Parse(textBefore);

                    if (state.State == XmlParser.ParserState.StartElement)
                    {
                        var closingState = XmlParser.Parse(editor.Document.GetText(0, Math.Max(0, editor.Offset - 2)));

                        var tagName = closingState.GetParentTagName(0);
                        
                        var caret = editor.Offset;

                        if (tagName != null)
                        {
                            editor.Document.Insert(caret, $"{tagName}>");
                            editor.Offset = caret + (tagName.Length + 1);
                        }
                        else
                        {
                            editor.Document.Insert(caret, ">");
                            editor.Offset = caret + (state.TagName == string.Empty ? 0 : 1);
                        }
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
