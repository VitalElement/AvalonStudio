namespace AvalonStudio.TextEditor.Document
{
    using System;
    using System.Globalization;
    using System.Text;

    static class NewLineFinder
    {
        static readonly char[] newline = { '\r', '\n' };

        internal static readonly string[] NewlineStrings = { "\r\n", "\r", "\n" };

        /// <summary>
        /// Gets the location of the next new line character, or SimpleSegment.Invalid
        /// if none is found.
        /// </summary>
        internal static SimpleSegment NextNewLine(string text, int offset)
        {
            int pos = text.IndexOfAny(newline, offset);
            if (pos >= 0)
            {
                if (text[pos] == '\r')
                {
                    if (pos + 1 < text.Length && text[pos + 1] == '\n')
                        return new SimpleSegment(pos, 2);
                }
                return new SimpleSegment(pos, 1);
            }
            return SimpleSegment.Invalid;
        }

        /// <summary>
        /// Gets the location of the next new line character, or SimpleSegment.Invalid
        /// if none is found.
        /// </summary>
        internal static SimpleSegment NextNewLine(ITextSource text, int offset)
        {
            int textLength = text.TextLength;
            int pos = text.IndexOfAny(newline, offset, textLength - offset);
            if (pos >= 0)
            {
                if (text.GetCharAt(pos) == '\r')
                {
                    if (pos + 1 < textLength && text.GetCharAt(pos + 1) == '\n')
                        return new SimpleSegment(pos, 2);
                }
                return new SimpleSegment(pos, 1);
            }
            return SimpleSegment.Invalid;
        }
    }

    partial class TextUtilities
    {
        //
        // Summary:
        //     Specifies a logical direction in which to perform certain text operations, such
        //     as inserting, retrieving, or navigating through text relative to a specified
        //     position (a System.Windows.Documents.TextPointer).
        public enum LogicalDirection
        {
            //
            // Summary:
            //     Backward, or from right to left.
            Backward = 0,
            //
            // Summary:
            //     Forward, or from left to right.
            Forward = 1
        }

        //
        // Summary:
        //     Classifies a character as whitespace, line terminator, part of an identifier,
        //     or other.
        public enum CharacterClass
        {
            //
            // Summary:
            //     The character is not whitespace, line terminator or part of an identifier.
            Other = 0,
            //
            // Summary:
            //     The character is whitespace (but not line terminator).
            Whitespace = 1,
            //
            // Summary:
            //     The character can be part of an identifier (Letter, digit or underscore).
            IdentifierPart = 2,
            //
            // Summary:
            //     The character is line terminator (\r or \n).
            LineTerminator = 3,
            //
            // Summary:
            //     The character is a unicode combining mark that modifies the previous character.
            //     Corresponds to the Unicode designations "Mn", "Mc" and "Me".
            CombiningMark = 4
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

        public static bool ContainsNumber(string text)
        {
            bool result = false;

            foreach (var character in text)
            {
                if (char.IsDigit(character))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Finds the next new line character starting at offset.
        /// </summary>
        /// <param name="text">The text source to search in.</param>
        /// <param name="offset">The starting offset for the search.</param>
        /// <param name="newLineType">The string representing the new line that was found, or null if no new line was found.</param>
        /// <returns>The position of the first new line starting at or after <paramref name="offset"/>,
        /// or -1 if no new line was found.</returns>
        public static int FindNextNewLine(ITextSource text, int offset, out string newLineType)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (offset < 0 || offset > text.TextLength)
                throw new ArgumentOutOfRangeException("offset", offset, "offset is outside of text source");
            SimpleSegment s = NewLineFinder.NextNewLine(text, offset);
            if (s == SimpleSegment.Invalid)
            {
                newLineType = null;
                return -1;
            }
            else
            {
                if (s.Length == 2)
                {
                    newLineType = "\r\n";
                }
                else if (text.GetCharAt(s.Offset) == '\n')
                {
                    newLineType = "\n";
                }
                else
                {
                    newLineType = "\r";
                }
                return s.Offset;
            }
        }

        /// <summary>
        /// Gets whether the specified string is a newline sequence.
        /// </summary>
        public static bool IsNewLine(string newLine)
        {
            return newLine == "\r\n" || newLine == "\n" || newLine == "\r";
        }

        /// <summary>
        /// Normalizes all new lines in <paramref name="input"/> to be <paramref name="newLine"/>.
        /// </summary>
        public static string NormalizeNewLines(string input, string newLine)
        {
            if (input == null)
                return null;
            if (!IsNewLine(newLine))
                throw new ArgumentException("newLine must be one of the known newline sequences");
            SimpleSegment ds = NewLineFinder.NextNewLine(input, 0);
            if (ds == SimpleSegment.Invalid) // text does not contain any new lines
                return input;
            StringBuilder b = new StringBuilder(input.Length);
            int lastEndOffset = 0;
            do
            {
                b.Append(input, lastEndOffset, ds.Offset - lastEndOffset);
                b.Append(newLine);
                lastEndOffset = ds.EndOffset;
                ds = NewLineFinder.NextNewLine(input, lastEndOffset);
            } while (ds != SimpleSegment.Invalid);
            // remaining string (after last newline)
            b.Append(input, lastEndOffset, input.Length - lastEndOffset);
            return b.ToString();
        }

        /// <summary>
        /// Gets the newline sequence used in the document at the specified line.
        /// </summary>
        public static string GetNewLineFromDocument(IDocument document, int lineNumber)
        {
            IDocumentLine line = document.GetLineByNumber(lineNumber);
            if (line.DelimiterLength == 0)
            {
                // at the end of the document, there's no line delimiter, so use the delimiter
                // from the previous line
                line = line.PreviousLine;
                if (line == null)
                    return Environment.NewLine;
            }
            return document.GetText(line.Offset + line.Length, line.DelimiterLength);
        }

        #region GetCharacterClass
        /// <summary>
        /// Gets whether the character is whitespace, part of an identifier, or line terminator.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "c")]
        public static CharacterClass GetCharacterClass(char c)
        {
            if (c == '\r' || c == '\n')
                return CharacterClass.LineTerminator;
            if (c == '_')
                return CharacterClass.IdentifierPart;
            return GetCharacterClass(char.GetUnicodeCategory(c));
        }

        static CharacterClass GetCharacterClass(char highSurrogate, char lowSurrogate)
        {
            if (char.IsSurrogatePair(highSurrogate, lowSurrogate))
            {
                return GetCharacterClass(char.GetUnicodeCategory(highSurrogate.ToString() + lowSurrogate.ToString(), 0));
            }
            else
            {
                // malformed surrogate pair
                return CharacterClass.Other;
            }
        }

        static CharacterClass GetCharacterClass(UnicodeCategory c)
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
        #endregion


        /// <summary>
        /// Gets the next caret position.
        /// </summary>
        /// <param name="textSource">The text source.</param>
        /// <param name="offset">The start offset inside the text source.</param>
        /// <param name="direction">The search direction (forwards or backwards).</param>
        /// <param name="mode">The mode for caret positioning.</param>
        /// <returns>The offset of the next caret position, or -1 if there is no further caret position
        /// in the text source.</returns>
        /// <remarks>
        /// This method is NOT equivalent to the actual caret movement when using VisualLine.GetNextCaretPosition.
        /// In real caret movement, there are additional caret stops at line starts and ends. This method
        /// treats linefeeds as simple whitespace.
        /// </remarks>
        public static int GetNextCaretPosition(ITextSource textSource, int offset, LogicalDirection direction, CaretPositioningMode mode)
        {
            if (textSource == null)
                throw new ArgumentNullException("textSource");
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
                    throw new ArgumentException("Unsupported CaretPositioningMode: " + mode, "mode");
            }
            if (direction != LogicalDirection.Backward
                && direction != LogicalDirection.Forward)
            {
                throw new ArgumentException("Invalid LogicalDirection: " + direction, "direction");
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

        static bool IsNormal(CaretPositioningMode mode)
        {
            return mode == CaretPositioningMode.Normal || mode == CaretPositioningMode.EveryCodepoint;
        }

        static bool StopBetweenCharacters(CaretPositioningMode mode, CharacterClass charBefore, CharacterClass charAfter)
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

    }

}
