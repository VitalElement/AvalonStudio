using System;
using System.Text;

namespace AvalonStudio.TextEditor.Document
{
	internal static class NewLineFinder
	{
		private static readonly char[] newline = {'\r', '\n'};

		internal static readonly string[] NewlineStrings = {"\r\n", "\r", "\n"};

		/// <summary>
		///     Gets the location of the next new line character, or SimpleSegment.Invalid
		///     if none is found.
		/// </summary>
		internal static SimpleSegment NextNewLine(string text, int offset)
		{
			var pos = text.IndexOfAny(newline, offset);
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
		///     Gets the location of the next new line character, or SimpleSegment.Invalid
		///     if none is found.
		/// </summary>
		internal static SimpleSegment NextNewLine(ITextSource text, int offset)
		{
			var textLength = text.TextLength;
			var pos = text.IndexOfAny(newline, offset, textLength - offset);
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
		/// <summary>
		///     Specifies the mode for getting the next caret position.
		/// </summary>
		public enum CaretPositioningMode
		{
			/// <summary>
			///     Normal positioning (stop after every grapheme)
			/// </summary>
			Normal,

			/// <summary>
			///     Stop only on word borders.
			/// </summary>
			WordBorder,

			/// <summary>
			///     Stop only at the beginning of words. This is used for Ctrl+Left/Ctrl+Right.
			/// </summary>
			WordStart,

			/// <summary>
			///     Stop only at the beginning of words, and anywhere in the middle of symbols.
			/// </summary>
			WordStartOrSymbol,

			/// <summary>
			///     Stop only on word borders, and anywhere in the middle of symbols.
			/// </summary>
			WordBorderOrSymbol,

			/// <summary>
			///     Stop between every Unicode codepoint, even within the same grapheme.
			///     This is used to implement deleting the previous grapheme when Backspace is pressed.
			/// </summary>
			EveryCodepoint
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

		public static bool ContainsNumber(string text)
		{
			var result = false;

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

		public static bool IsSymbol(string text)
		{
			var result = false;

			if (!string.IsNullOrEmpty(text) && (char.IsLetter(text[0]) || text[0] == '_'))
			{
				result = true;
			}

			return result;
		}

		/// <summary>
		///     Finds the next new line character starting at offset.
		/// </summary>
		/// <param name="text">The text source to search in.</param>
		/// <param name="offset">The starting offset for the search.</param>
		/// <param name="newLineType">The string representing the new line that was found, or null if no new line was found.</param>
		/// <returns>
		///     The position of the first new line starting at or after <paramref name="offset" />,
		///     or -1 if no new line was found.
		/// </returns>
		public static int FindNextNewLine(ITextSource text, int offset, out string newLineType)
		{
			if (text == null)
				throw new ArgumentNullException("text");
			if (offset < 0 || offset > text.TextLength)
				throw new ArgumentOutOfRangeException("offset", offset, "offset is outside of text source");
			var s = NewLineFinder.NextNewLine(text, offset);
			if (s == SimpleSegment.Invalid)
			{
				newLineType = null;
				return -1;
			}
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

		/// <summary>
		///     Gets whether the specified string is a newline sequence.
		/// </summary>
		public static bool IsNewLine(string newLine)
		{
			return newLine == "\r\n" || newLine == "\n" || newLine == "\r";
		}

		/// <summary>
		///     Normalizes all new lines in <paramref name="input" /> to be <paramref name="newLine" />.
		/// </summary>
		public static string NormalizeNewLines(string input, string newLine)
		{
			if (input == null)
				return null;
			if (!IsNewLine(newLine))
				throw new ArgumentException("newLine must be one of the known newline sequences");
			var ds = NewLineFinder.NextNewLine(input, 0);
			if (ds == SimpleSegment.Invalid) // text does not contain any new lines
				return input;
			var b = new StringBuilder(input.Length);
			var lastEndOffset = 0;
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
		///     Gets the newline sequence used in the document at the specified line.
		/// </summary>
		public static string GetNewLineFromDocument(IDocument document, int lineNumber)
		{
			var line = document.GetLineByNumber(lineNumber);
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
	}
}