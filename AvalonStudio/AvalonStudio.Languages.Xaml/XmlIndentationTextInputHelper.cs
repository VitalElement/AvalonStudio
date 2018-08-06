using Avalonia.Ide.CompletionEngine;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Languages.Xaml
{
    public class XmlIndentationTextInputHelper : ITextEditorInputHelper
    {
        public bool AfterTextInput(ITextEditor editor, string inputText)
        {
            if (inputText == "\n")
            {
                var textBefore = editor.Document.GetText(0, Math.Max(0, editor.Offset));

                var parser = XmlParser.Parse(textBefore);

                switch (parser.State)
                {
                    case XmlParser.ParserState.None:
                        var prevTagWhitespace = editor.Document.GetWhitespaceBefore(parser.ContainingTagStart);

                        var currentLineWhitespace = editor.Document.GetWhitespaceAfter(editor.CurrentLine().Offset);

                        if (parser.NestingLevel > 0)
                        {
                            editor.Document.Replace(currentLineWhitespace,
                                editor.Document.GetText(prevTagWhitespace) +
                                new string(' ', 2));
                        }
                        else
                        {
                            editor.Document.Replace(currentLineWhitespace,
                                editor.Document.GetText(prevTagWhitespace));
                        }

                        if(editor.Offset < editor.Document.TextLength && editor.Document.Text[editor.Offset] == '<')
                        {
                            editor.Document.Insert(editor.Offset, "\n" + editor.Document.GetText(prevTagWhitespace));

                            editor.Offset = editor.PreviousLine().Offset + editor.PreviousLine().Length;
                        }
                        break;
                }
            }

            return false;
        }

        public bool BeforeTextInput(ITextEditor editor, string inputText)
        {
            return false;
        }

        public void CaretMovedToEmptyLine(ITextEditor editor)
        {
        }
    }
}
