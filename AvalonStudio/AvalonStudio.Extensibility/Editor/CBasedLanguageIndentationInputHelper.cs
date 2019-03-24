using AvalonStudio.Documents;

namespace AvalonStudio.Editor
{
    public class CBasedLanguageIndentationInputHelper : ITextEditorInputHelper
    {
        private (ISegment whitespace, int offset, char character) GetPreviousBracketInfo(ITextEditor editor, int offset)
        {
            if(offset >= editor.Document.TextLength)
            {
                offset = editor.Document.TextLength - 1;
            }

            if (offset >= 0)
            {
                var location = editor.Document.GetLocation(offset);
                var line = editor.Document.Lines[location.Line];

                var previousBracket = editor.Document.GetLastCharMatching(c => c == '{' || c == '}', line.Offset, 0);

                if (previousBracket.index != -1)
                {
                    var previousBracketLocation = editor.Document.GetLocation(previousBracket.index);
                    var previousBracketLine = editor.Document.Lines[previousBracketLocation.Line];

                    return (editor.Document.GetWhitespaceAfter(previousBracketLine.Offset), previousBracket.index, previousBracket.character);
                }
            }

            return (null, -1, '\0');
        }

        /// <summary>
        /// Indents a line to the same indentation as the last { or } + 1 indentation. Or 0 indentations if no previous brackets.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="line">The line number to indent.</param>
        /// <param name="previousBracketWhitespace">The whitespace before the last bracket.</param>
        /// <param name="previousBracketChar">The last bracket char.</param>
        private void Indent(ITextEditor editor, int line, ISegment previousBracketWhitespace, char? previousBracketChar, int indentations = 1, bool indentForClosing = false)
        {
            var whiteSpace = editor.Document.GetWhitespaceAfter(editor.Document.Lines[line].Offset);

            if (previousBracketWhitespace != null)
            {
                if (indentForClosing)
                {
                    editor.Document.Replace(whiteSpace, editor.Document.GetText(previousBracketWhitespace) + (previousBracketChar == '{' || previousBracketChar == '}' ? new string(' ', 4 * indentations) : ""));
                }
                else
                {
                    editor.Document.Replace(whiteSpace, editor.Document.GetText(previousBracketWhitespace) + (previousBracketChar == '{' ? new string(' ', 4 * indentations) : ""));
                }
            }
            else
            {
                editor.Document.Replace(whiteSpace, "");
            }
        }

        /// <summary>
        /// Indents the current line assuming we already have normal indentation, applying syntax specific rules.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="onEnter">True if called after user pressed enter.</param>
        private void ConditionalIndent(ITextEditor editor, bool onEnter = false)
        {
            var currentLine = editor.CurrentLine();

            if (currentLine.PreviousLine != null)
            {
                var lastCharInfo = editor.Document.GetLastNonWhiteSpaceCharBefore(currentLine.PreviousLine.EndOffset, currentLine.PreviousLine.Offset);

                if (lastCharInfo.index != -1)
                {
                    if (lastCharInfo.character == '{' && onEnter)
                    {
                        var whiteSpace = editor.Document.GetWhitespaceAfter(editor.PreviousLine().Offset);

                        var currentChar = editor.Document.GetCharAt(editor.Offset);

                        if (char.IsWhiteSpace(currentChar) || currentChar == '}')
                        {
                            editor.Document.Insert(editor.Offset, "\n" + editor.Document.GetText(whiteSpace));

                            editor.Offset = editor.PreviousLine().EndOffset;
                        }
                    }
                    else if (lastCharInfo.character == ')')
                    {
                        var lineText = editor.CurrentLineText();

                        if (!lineText.Contains("{"))
                        {
                            editor.Document.Insert(editor.Offset, new string(' ', 4));
                        }
                    }
                    else
                    {
                        var previousLineText = editor.PreviousLineText().Trim();

                        if (previousLineText.EndsWith("else"))
                        {
                            var lineText = editor.CurrentLineText();

                            if (!lineText.Contains("{"))
                            {
                                editor.Document.Insert(editor.Offset, new string(' ', 4));
                            }
                        }
                    }
                }
            }
        }

        public bool AfterTextInput(ITextEditor editor, string inputText)
        {
            if (inputText == "\n")
            {
                var previousBracketInfo = GetPreviousBracketInfo(editor, editor.Offset - 1);

                if (previousBracketInfo.whitespace != null)
                {
                    Indent(editor, editor.Line, previousBracketInfo.whitespace, previousBracketInfo.character);
                }

                ConditionalIndent(editor, true);
            }
            else if (inputText == "{" || inputText == "}" || inputText == ";")
            {
                var prevLine = editor.PreviousLine();

                if (prevLine != null)
                {
                    var lastCharInfo = editor.Document.GetLastNonWhiteSpaceCharBefore(prevLine.EndOffset, prevLine.Offset);

                    if (!(inputText == "{" && lastCharInfo.character == '{'))
                    {
                        if (inputText == "{" || inputText == "}")
                        {
                            var lastBracketWhitespaceInfo = GetPreviousBracketInfo(editor, editor.Offset - 2);

                            Indent(editor, editor.Line, lastBracketWhitespaceInfo.whitespace, lastBracketWhitespaceInfo.character);
                        }
                        else
                        {
                            var lastBracketWhitespaceInfo = GetPreviousBracketInfo(editor, editor.Offset - 1);
                            
                            if (lastCharInfo.character == ')')
                            {
                                Indent(editor, editor.Line, lastBracketWhitespaceInfo.whitespace, lastBracketWhitespaceInfo.character, lastBracketWhitespaceInfo.character == '{' ? 2 : 1, true);
                            }
                            else
                            {
                                var previousLineText = editor.PreviousLineText().Trim();

                                if (previousLineText.EndsWith("else"))
                                {
                                    var lineText = editor.CurrentLineText();

                                    if (!lineText.Contains("{"))
                                    {
                                        Indent(editor, editor.Line, lastBracketWhitespaceInfo.whitespace, lastBracketWhitespaceInfo.character, lastBracketWhitespaceInfo.character == '{' ? 2 : 1, true);
                                    }
                                }
                                else
                                {
                                    Indent(editor, editor.Line, lastBracketWhitespaceInfo.whitespace, lastBracketWhitespaceInfo.character);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Indent(editor, editor.Line, null, null);
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
            if (editor.Document.TextLength > 0)
            {
                var previousBracketInfo = GetPreviousBracketInfo(editor, editor.Offset < editor.Document.TextLength ? editor.Offset : editor.Offset - 1);

                Indent(editor, editor.Line, previousBracketInfo.whitespace, previousBracketInfo.character);

                ConditionalIndent(editor);

                editor.Offset = editor.CurrentLine().EndOffset;
            }
        }
    }
}
