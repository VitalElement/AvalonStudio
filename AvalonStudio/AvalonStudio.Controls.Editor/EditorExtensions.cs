using Avalonia.Controls;
using AvalonStudio.Documents;
using System;

namespace AvalonStudio.Controls.Editor
{
    public static class EditorExtensions
    {
        public static T FindAncestorByType<T>(this IControl control)
            where T : IControl
        {
            while (control != null && !(control is T))
            {
                control = control.Parent;
            }

            return (T)control;
        }

        public static void HookupLoadedUnloadedAction(this IControl element, Action<bool> action)
        {
            if (element.IsAttachedToVisualTree)
            {
                action(true);
            }

            element.AttachedToVisualTree += (o, e) => action(true);
            element.DetachedFromVisualTree += (o, e) => action(false);
        }

        public static void AttachLocationChanged(this Window topLevel, EventHandler<PixelPointEventArgs> handler)
        {
            topLevel.PositionChanged += handler;
        }

        public static void DetachLocationChanged(this Window topLevel, EventHandler<PixelPointEventArgs> handler)
        {
            topLevel.PositionChanged -= handler;
        }

        public static string GetPreviousWordAtIndex(this ITextDocument document, int index)
        {
            var lastWordIndex = TextUtilities.GetNextCaretPosition(document, index, LogicalDirection.Backward, CaretPositioningMode.WordBorder);

            if (lastWordIndex >= 0 && document.GetLocation(lastWordIndex).Line == document.GetLocation(index).Line)
            {
                return document.GetWordAtIndex(lastWordIndex);
            }
            else
            {
                return document.GetWordAtIndex(index);
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

        public static int GetIntellisenseStartPosition(this ITextDocument textSource, int offset, Predicate<char> isValidChar)
        {
            while (offset > 0)
            {
                var currentChar = textSource.GetCharAt(offset - 1);

                if (!isValidChar(currentChar))
                {
                    break;
                }

                offset--;
            }

            return offset;
        }

        public static string GetWordAtIndex(this ITextDocument document, int index)
        {
            var result = string.Empty;

            if (index >= 0 && document.TextLength > index)
            {
                var start = index;

                var currentChar = document.GetCharAt(index);
                var prevChar = '\0';

                if (index > 0)
                {
                    prevChar = document.GetCharAt(index - 1);
                }

                var charClass = TextUtilities.GetCharacterClass(currentChar);

                if (charClass != CharacterClass.LineTerminator && prevChar != ' ' && TextUtilities.GetCharacterClass(prevChar) != CharacterClass.LineTerminator)
                {
                    start = TextUtilities.GetNextCaretPosition(document, index, LogicalDirection.Backward, CaretPositioningMode.WordStart);
                }

                var end = TextUtilities.GetNextCaretPosition(document, start, LogicalDirection.Forward, CaretPositioningMode.WordBorder);

                if (start != -1 && end != -1)
                {
                    var word = document.GetText(start, end - start).Trim();

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
