using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace AvalonStudio.Documents
{
    public enum LogicalDirection
    {
        Backward,
        Forward
    }

    /// <summary>
    /// Specifies the mode for getting the next caret position.
    /// </summary>
    public enum CaretPositioningMode
    {
        /// <summary>
        /// Normal positioning (stop after every grapheme)
        /// </summary>
        Normal,
        /// <summary>
        /// Stop only on word borders.
        /// </summary>
        WordBorder,
        /// <summary>
        /// Stop only at the beginning of words. This is used for Ctrl+Left/Ctrl+Right.
        /// </summary>
        WordStart,
        /// <summary>
        /// Stop only at the beginning of words, and anywhere in the middle of symbols.
        /// </summary>
        WordStartOrSymbol,
        /// <summary>
        /// Stop only on word borders, and anywhere in the middle of symbols.
        /// </summary>
        WordBorderOrSymbol,
        /// <summary>
        /// Stop between every Unicode codepoint, even within the same grapheme.
        /// This is used to implement deleting the previous grapheme when Backspace is pressed.
        /// </summary>
        EveryCodepoint
    }

    /// <summary>
    /// Classifies a character as whitespace, line terminator, part of an identifier, or other.
    /// </summary>
    public enum CharacterClass
    {
        /// <summary>
        /// The character is not whitespace, line terminator or part of an identifier.
        /// </summary>
        Other,
        /// <summary>
        /// The character is whitespace (but not line terminator).
        /// </summary>        
        Whitespace,
        /// <summary>
        /// The character can be part of an identifier (Letter, digit or underscore).
        /// </summary>
        IdentifierPart,
        /// <summary>
        /// The character is line terminator (\r or \n).
        /// </summary>
        LineTerminator,
        /// <summary>
        /// The character is a unicode combining mark that modifies the previous character.
        /// Corresponds to the Unicode designations "Mn", "Mc" and "Me".
        /// </summary>
        CombiningMark
    }

    public static class TextUtilities
    {
        private static bool IsNormal(CaretPositioningMode mode)
        {
            return mode == CaretPositioningMode.Normal || mode == CaretPositioningMode.EveryCodepoint;
        }

        public static CharacterClass GetCharacterClass(char c)
        {
            if (c == '\r' || c == '\n')
                return CharacterClass.LineTerminator;
            if (c == '_')
                return CharacterClass.IdentifierPart;
            return GetCharacterClass(GetUnicodeCategory(c));
        }

        private static CharacterClass GetCharacterClass(char highSurrogate, char lowSurrogate)
        {
            if (char.IsSurrogatePair(highSurrogate, lowSurrogate))
            {
                return GetCharacterClass(GetUnicodeCategoryString(highSurrogate.ToString() + lowSurrogate, 0));
            }
            // malformed surrogate pair
            return CharacterClass.Other;
        }

        private static readonly Func<char, UnicodeCategory> GetUnicodeCategory = (Func<char, UnicodeCategory>)typeof(char)
            .GetRuntimeMethod("GetUnicodeCategory", new[] { typeof(char) })
            .CreateDelegate(typeof(Func<char, UnicodeCategory>));

        private static readonly Func<string, int, UnicodeCategory> GetUnicodeCategoryString = (Func<string, int, UnicodeCategory>)typeof(char)
            .GetRuntimeMethod("GetUnicodeCategory", new[] { typeof(string), typeof(int) })
            .CreateDelegate(typeof(Func<string, int, UnicodeCategory>));

        private static CharacterClass GetCharacterClass(UnicodeCategory c)
        {
            switch (c)
            {
                case UnicodeCategory.SpaceSeparator:
                case UnicodeCategory.LineSeparator:
                case UnicodeCategory.ParagraphSeparator:
                case UnicodeCategory.Control:
                    return CharacterClass.Whitespace;
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                case UnicodeCategory.ModifierLetter:
                case UnicodeCategory.OtherLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    return CharacterClass.IdentifierPart;
                case UnicodeCategory.NonSpacingMark:
                case UnicodeCategory.SpacingCombiningMark:
                case UnicodeCategory.EnclosingMark:
                    return CharacterClass.CombiningMark;
                default:
                    return CharacterClass.Other;
            }
        }

        private static bool StopBetweenCharacters(CaretPositioningMode mode, CharacterClass charBefore, CharacterClass charAfter)
        {
            if (mode == CaretPositioningMode.EveryCodepoint)
                return true;
            // Don't stop in the middle of a grapheme
            if (charAfter == CharacterClass.CombiningMark)
                return false;
            // Stop after every grapheme in normal mode
            if (mode == CaretPositioningMode.Normal)
                return true;
            if (charBefore == charAfter)
            {
                if (charBefore == CharacterClass.Other &&
                    (mode == CaretPositioningMode.WordBorderOrSymbol || mode == CaretPositioningMode.WordStartOrSymbol))
                {
                    // With the "OrSymbol" modes, there's a word border and start between any two unknown characters
                    return true;
                }
            }
            else
            {
                // this looks like a possible border

                // if we're looking for word starts, check that this is a word start (and not a word end)
                // if we're just checking for word borders, accept unconditionally
                if (!((mode == CaretPositioningMode.WordStart || mode == CaretPositioningMode.WordStartOrSymbol)
                      && (charAfter == CharacterClass.Whitespace || charAfter == CharacterClass.LineTerminator)))
                {
                    return true;
                }
            }
            return false;
        }

        public static int GetNextCaretPosition(ITextDocument textSource, int offset, LogicalDirection direction, CaretPositioningMode mode)
        {
            if (textSource == null)
                throw new ArgumentNullException(nameof(textSource));
            switch (mode)
            {
                case CaretPositioningMode.Normal:
                case CaretPositioningMode.EveryCodepoint:
                case CaretPositioningMode.WordBorder:
                case CaretPositioningMode.WordBorderOrSymbol:
                case CaretPositioningMode.WordStart:
                case CaretPositioningMode.WordStartOrSymbol:
                    break; // OK
                default:
                    throw new ArgumentException("Unsupported CaretPositioningMode: " + mode, nameof(mode));
            }
            if (direction != LogicalDirection.Backward
                && direction != LogicalDirection.Forward)
            {
                throw new ArgumentException("Invalid LogicalDirection: " + direction, nameof(direction));
            }
            int textLength = textSource.TextLength;
            if (textLength <= 0)
            {
                // empty document? has a normal caret position at 0, though no word borders
                if (IsNormal(mode))
                {
                    if (offset > 0 && direction == LogicalDirection.Backward) return 0;
                    if (offset < 0 && direction == LogicalDirection.Forward) return 0;
                }
                return -1;
            }
            while (true)
            {
                int nextPos = (direction == LogicalDirection.Backward) ? offset - 1 : offset + 1;

                // return -1 if there is no further caret position in the text source
                // we also need this to handle offset values outside the valid range
                if (nextPos < 0 || nextPos > textLength)
                    return -1;

                // check if we've run against the textSource borders.
                // a 'textSource' usually isn't the whole document, but a single VisualLineElement.
                if (nextPos == 0)
                {
                    // at the document start, there's only a word border
                    // if the first character is not whitespace
                    if (IsNormal(mode) || !char.IsWhiteSpace(textSource.GetCharAt(0)))
                        return nextPos;
                }
                else if (nextPos == textLength)
                {
                    // at the document end, there's never a word start
                    if (mode != CaretPositioningMode.WordStart && mode != CaretPositioningMode.WordStartOrSymbol)
                    {
                        // at the document end, there's only a word border
                        // if the last character is not whitespace
                        if (IsNormal(mode) || !char.IsWhiteSpace(textSource.GetCharAt(textLength - 1)))
                            return nextPos;
                    }
                }
                else
                {
                    char charBefore = textSource.GetCharAt(nextPos - 1);
                    char charAfter = textSource.GetCharAt(nextPos);
                    // Don't stop in the middle of a surrogate pair
                    if (!char.IsSurrogatePair(charBefore, charAfter))
                    {
                        CharacterClass classBefore = GetCharacterClass(charBefore);
                        CharacterClass classAfter = GetCharacterClass(charAfter);
                        // get correct class for characters outside BMP:
                        if (char.IsLowSurrogate(charBefore) && nextPos >= 2)
                        {
                            classBefore = GetCharacterClass(textSource.GetCharAt(nextPos - 2), charBefore);
                        }
                        if (char.IsHighSurrogate(charAfter) && nextPos + 1 < textLength)
                        {
                            classAfter = GetCharacterClass(charAfter, textSource.GetCharAt(nextPos + 1));
                        }
                        if (StopBetweenCharacters(mode, classBefore, classAfter))
                        {
                            return nextPos;
                        }
                    }
                }
                // we'll have to continue searching...
                offset = nextPos;
            }
        }

        public static ISegment GetLeadingWhitespace(ITextDocument document, IDocumentLine documentLine)
        {
            if (documentLine == null)
                throw new ArgumentNullException(nameof(documentLine));

            return document.GetWhitespaceAfter(documentLine.Offset);
        }

        /// <summary>
        /// Gets the trailing whitespace segment on the document line.
        /// </summary>        
        public static ISegment GetTrailingWhitespace(ITextDocument document, IDocumentLine documentLine)
        {
            if (documentLine == null)
                throw new ArgumentNullException(nameof(documentLine));
            ISegment segment = document.GetWhitespaceBefore(documentLine.EndOffset);
            // If the whole line consists of whitespace, we consider all of it as leading whitespace,
            // so return an empty segment as trailing whitespace.
            if (segment.Offset == documentLine.Offset)
                return new SimpleSegment(documentLine.EndOffset, 0);
            else
                return segment;
        }
    }
}
