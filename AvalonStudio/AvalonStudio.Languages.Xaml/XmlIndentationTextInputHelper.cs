using Avalonia.Ide.CompletionEngine;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using System;

namespace AvalonStudio.Languages.Xaml
{
    public class XmlIndentationTextInputHelper : ITextEditorInputHelper
    {
        public static XmlParser Indent (ITextEditor editor, int offset)
        {
            var textBefore = editor.Document.GetText(0, Math.Max(0, offset));

            var parser = XmlParser.Parse(textBefore);

            switch (parser.State)
            {
                case XmlParser.ParserState.None:
                    {
                        var line = editor.Document.GetLineByNumber(editor.Document.GetLocation(offset).Line);

                        var nextText = editor.Document.GetText(offset, line.EndOffset - offset);

                        int nestOffset = 0;

                        if(nextText.Contains("</"))
                        {
                            nestOffset = -1;
                        }

                        var currentLineWhitespace = editor.Document.GetWhitespaceAfter(line.Offset);

                        editor.Document.Replace(currentLineWhitespace, new string(' ', 2 * (parser.NestingLevel + nestOffset)));
                    }
                    break;

                case XmlParser.ParserState.InsideElement:
                    {
                        var line = editor.Document.GetLineByNumber(editor.Document.GetLocation(offset).Line);

                        var currentLineWhitespace = editor.Document.GetWhitespaceAfter(line.Offset);

                        var location = editor.Document.GetLocation(parser.ElementNameEnd.Value);
                        line = editor.Document.GetLineByNumber(location.Line);

                        var whitespace = parser.ElementNameEnd - line.Offset;

                        editor.Document.Replace(currentLineWhitespace, new string(' ', whitespace.Value + 2));
                    }
                    break;
            }

            return parser;
        }

        public bool AfterTextInput(ITextEditor editor, string inputText)
        {
            switch (inputText)
            {
                case "<":
                case ">":
                case "/":
                    {
                        Indent(editor, editor.CurrentLine().Offset);
                    }
                    break;

                case "\n":
                    var textBefore = editor.Document.GetText(0, Math.Max(0, editor.CurrentLine().Offset));
                    var parser = XmlParser.Parse(textBefore);

                    if (parser.State == XmlParser.ParserState.None && inputText == "\n")
                    {
                        if (editor.Offset < editor.Document.TextLength && editor.Document.Text[editor.Offset] == '<')
                        {
                            var line = editor.CurrentLine();
                            var currentLineWhitespace = editor.Document.GetWhitespaceAfter(line.Offset);

                            editor.Document.Replace(currentLineWhitespace, new string(' ', 2 * parser.NestingLevel) + "\n");

                            Indent(editor, editor.CurrentLine().Offset);
                            editor.Offset = editor.PreviousLine().Offset + editor.PreviousLine().Length;
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
            Indent(editor, editor.CurrentLine().Offset);
        }
    }
}
