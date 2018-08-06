using Avalonia.Ide.CompletionEngine;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using System;

namespace AvalonStudio.Languages.Xaml
{
    public class XmlIndentationTextInputHelper : ITextEditorInputHelper
    {
        private void Indent (ITextEditor editor, XmlParser parser)
        {
            switch (parser.State)
            {
                case XmlParser.ParserState.None:
                    {
                        var currentLineWhitespace = editor.Document.GetWhitespaceAfter(editor.CurrentLine().Offset);

                        editor.Document.Replace(currentLineWhitespace, new string(' ', 2 * parser.NestingLevel));
                    }
                    break;

                case XmlParser.ParserState.InsideElement:
                    {
                        var currentLineWhitespace = editor.Document.GetWhitespaceAfter(editor.CurrentLine().Offset);

                        var location = editor.Document.GetLocation(parser.ElementNameEnd.Value);
                        var line = editor.Document.GetLineByNumber(location.Line);

                        var whitespace = parser.ElementNameEnd - line.Offset;

                        editor.Document.Replace(currentLineWhitespace, new string(' ', whitespace.Value + 2));
                    }
                    break;
            }
        }

        public bool AfterTextInput(ITextEditor editor, string inputText)
        {
            switch (inputText)
            {
                case "\n":
                case ">":
                case "/":
                    {
                        var textBefore = editor.Document.GetText(0, Math.Max(0, editor.CurrentLine().Offset));

                        var parser = XmlParser.Parse(textBefore);

                        Indent(editor, parser);

                        if(parser.State == XmlParser.ParserState.None && inputText == "\n")
                        {
                            if (editor.Offset < editor.Document.TextLength && editor.Document.Text[editor.Offset] == '<')
                            {
                                editor.Document.Insert(editor.Offset, "\n" + new string(' ', 2 * (parser.NestingLevel - 1)));

                                editor.Offset = editor.PreviousLine().Offset + editor.PreviousLine().Length;
                            }
                        }
                    }
                    break;
            }

            return false;
        }

        public bool BeforeTextInput(ITextEditor editor, string inputText)
        {
            return false;
        }

        public void CaretMovedToEmptyLine(ITextEditor editor)
        {
            var textBefore = editor.Document.GetText(0, Math.Max(0, editor.CurrentLine().Offset));

            var parser = XmlParser.Parse(textBefore);

            Indent(editor, parser);
        }
    }
}
