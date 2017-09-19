using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public static class EditorExtensions
    {
        public static string GetPreviousWordAtIndex(this AvaloniaEdit.TextEditor editor, int index)
        {
            var lastWordIndex = TextUtilities.GetNextCaretPosition(editor.Document, index, LogicalDirection.Backward, CaretPositioningMode.WordBorder);

            if (lastWordIndex >= 0 && editor.Document.GetLocation(lastWordIndex).Line == editor.Document.GetLocation(index).Line)
            {
                return editor.GetWordAtIndex(lastWordIndex);
            }
            else
            {
                return editor.GetWordAtIndex(index);
            }
        }

        public static bool IsSymbol(this string text)
        {
            var result = false;

            if (!string.IsNullOrEmpty(text) && (char.IsLetter(text[0]) || text[0] == '_'))
            {
                result = true;
            }

            return result;
        }

        public static int GetIntellisenseStartPosition(this ITextSource textSource, int offset)
        {
            while (true)
            {
                var currentChar = textSource.GetCharAt(offset - 1);

                if (!char.IsLetterOrDigit(currentChar) || offset < 0)
                {
                    break;
                }

                offset--;
            }

            return offset;
        }

        public static string GetWordAtIndex(this AvaloniaEdit.TextEditor editor, int index)
        {
            var result = string.Empty;

            if (index >= 0 && editor.Document.TextLength > index)
            {
                var start = index;

                var currentChar = editor.Document.GetCharAt(index);
                var prevChar = '\0';

                if (index > 0)
                {
                    prevChar = editor.Document.GetCharAt(index - 1);
                }

                var charClass = TextUtilities.GetCharacterClass(currentChar);

                if (charClass != CharacterClass.LineTerminator && prevChar != ' ' && TextUtilities.GetCharacterClass(prevChar) != CharacterClass.LineTerminator)
                {
                    start = TextUtilities.GetNextCaretPosition(editor.Document, index, LogicalDirection.Backward, CaretPositioningMode.WordStart);
                }

                var end = TextUtilities.GetNextCaretPosition(editor.Document, start, LogicalDirection.Forward, CaretPositioningMode.WordBorder);

                if (start != -1 && end != -1)
                {
                    var word = editor.Document.GetText(start, end - start).Trim();

                    if (word.IsSymbol())
                    {
                        result = word;
                    }
                }
            }

            return result;
        }
    }
}
