using Avalonia.Input;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace AvalonStudio.Documents
{
    public static class ITextDocumentExtensions
    {
        public static void TrimTrailingWhiteSpace(this ITextDocument document, ISegment line)
        {
            document.Replace(line, document.GetText(line).TrimEnd());
        }

        public static void TrimTrailingWhiteSpace(this ITextDocument document, int lineNumber)
        {
            var line = document.GetLineByNumber(lineNumber);
            document.TrimTrailingWhiteSpace(line);
        }

        public static void TrimTrailingWhiteSpace(this ITextDocument document)
        {
            using (document.RunUpdate())
            {
                foreach (var line in document.Lines)
                {
                    TrimTrailingWhiteSpace(document, line);
                }
            }
        }

        public static string GetText(this ITextDocument document, ISegment segment)
        {
            return document.GetText(segment.Offset, segment.Length);
        }

        public static void Replace(this ITextDocument document, ISegment segment, string text)
        {
            document.Replace(segment.Offset, segment.Length, text);
        }

        public static ISegment GetWhitespaceAfter(this ITextDocument textSource, int offset)
        {
            if (textSource == null)
                throw new ArgumentNullException(nameof(textSource));
            int pos;
            for (pos = offset; pos < textSource.TextLength; pos++)
            {
                char c = textSource.GetCharAt(pos);
                if (c != ' ' && c != '\t')
                    break;
            }

            return new SimpleSegment(offset, pos - offset);
        }

        public static ISegment GetWhitespaceBefore(this ITextDocument textSource, int offset)
        {
            if (textSource == null)
                throw new ArgumentNullException(nameof(textSource));
            int pos;
            for (pos = offset - 1; pos >= 0; pos--)
            {
                char c = textSource.GetCharAt(pos);
                if (c != ' ' && c != '\t')
                    break;
            }
            pos++; // go back the one character that isn't whitespace
            return new SimpleSegment(pos, offset - pos);
        }
    }

    /// <summary>
    /// An (Offset,Length)-pair.
    /// </summary>
    public interface ISegment
    {
        /// <summary>
        /// Gets the start offset of the segment.
        /// </summary>
        int Offset { get; }

        /// <summary>
        /// Gets the length of the segment.
        /// </summary>
        /// <remarks>For line segments (IDocumentLine), the length does not include the line delimeter.</remarks>
        int Length { get; }

        /// <summary>
        /// Gets the end offset of the segment.
        /// </summary>
        /// <remarks>EndOffset = Offset + Length;</remarks>
        int EndOffset { get; }
    }

    /// <summary>
    /// A line inside a <see cref="IDocument"/>.
    /// </summary>
    public interface IDocumentLine : ISegment
    {
        /// <summary>
        /// Gets the length of this line, including the line delimiter.
        /// </summary>
        int TotalLength { get; }

        /// <summary>
        /// Gets the length of the line terminator.
        /// Returns 1 or 2; or 0 at the end of the document.
        /// </summary>
        int DelimiterLength { get; }

        /// <summary>
        /// Gets the number of this line.
        /// The first line has the number 1.
        /// </summary>
        int LineNumber { get; }

        /// <summary>
        /// Gets the previous line. Returns null if this is the first line in the document.
        /// </summary>
        IDocumentLine PreviousLine { get; }

        /// <summary>
        /// Gets the next line. Returns null if this is the last line in the document.
        /// </summary>
        IDocumentLine NextLine { get; }
    }

    public interface IIndexableList<T> : IEnumerable<T>
    {
        T this[int index] { get; }

        int Count { get; }
    }

    public struct TextLocation : IComparable<TextLocation>, IEquatable<TextLocation>
    {
        /// <summary>
        /// Represents no text location (0, 0).
        /// </summary>
        public static readonly TextLocation Empty = new TextLocation(0, 0);

        /// <summary>
        /// Creates a TextLocation instance.
        /// </summary>
        public TextLocation(int line, int column)
        {
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Gets the line number.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the column number.
        /// </summary>
        public int Column { get; }

        /// <summary>
        /// Gets whether the TextLocation instance is empty.
        /// </summary>
        public bool IsEmpty => Column <= 0 && Line <= 0;

        /// <summary>
        /// Gets a string representation for debugging purposes.
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "(Line {1}, Col {0})", Column, Line);
        }

        /// <summary>
        /// Gets a hash code.
        /// </summary>
        public override int GetHashCode()
        {
            return unchecked(191 * Column.GetHashCode() ^ Line.GetHashCode());
        }

        /// <summary>
        /// Equality test.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is TextLocation)) return false;
            return (TextLocation)obj == this;
        }

        /// <summary>
        /// Equality test.
        /// </summary>
        public bool Equals(TextLocation other)
        {
            return this == other;
        }

        /// <summary>
        /// Equality test.
        /// </summary>
        public static bool operator ==(TextLocation left, TextLocation right)
        {
            return left.Column == right.Column && left.Line == right.Line;
        }

        /// <summary>
        /// Inequality test.
        /// </summary>
        public static bool operator !=(TextLocation left, TextLocation right)
        {
            return left.Column != right.Column || left.Line != right.Line;
        }

        /// <summary>
        /// Compares two text locations.
        /// </summary>
        public static bool operator <(TextLocation left, TextLocation right)
        {
            if (left.Line < right.Line)
                return true;
            if (left.Line == right.Line)
                return left.Column < right.Column;
            return false;
        }

        /// <summary>
        /// Compares two text locations.
        /// </summary>
        public static bool operator >(TextLocation left, TextLocation right)
        {
            if (left.Line > right.Line)
                return true;
            if (left.Line == right.Line)
                return left.Column > right.Column;
            return false;
        }

        /// <summary>
        /// Compares two text locations.
        /// </summary>
        public static bool operator <=(TextLocation left, TextLocation right)
        {
            return !(left > right);
        }

        /// <summary>
        /// Compares two text locations.
        /// </summary>
        public static bool operator >=(TextLocation left, TextLocation right)
        {
            return !(left < right);
        }

        /// <summary>
        /// Compares two text locations.
        /// </summary>
        public int CompareTo(TextLocation other)
        {
            if (this == other)
                return 0;
            if (this < other)
                return -1;
            return 1;
        }
    }

    public class DocumentChangeEventArgs : EventArgs
    {
        public DocumentChangeEventArgs(int offset, string removedText, string insertedText)
        {
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), offset, "offset must not be negative");
            Offset = offset;
            RemovedText = removedText;
            InsertedText = insertedText;
        }

        public int Offset { get; }

        /// <summary>
        /// The text that was removed.
        /// </summary>
        public string RemovedText { get; }

        /// <summary>
        /// The number of characters removed.
        /// </summary>
        public int RemovalLength => RemovedText.Length;

        /// <summary>
        /// The text that was inserted.
        /// </summary>
        public string InsertedText { get; }

        /// <summary>
        /// The number of characters inserted.
        /// </summary>
        public int InsertionLength => InsertedText.Length;
    }

    public interface ITextDocument
    {
        void Insert(int offset, string text);

        void Replace(int offset, int length, string text);

        TextLocation GetLocation(int offset);

        string Text { get; }

        int TextLength { get; }

        IIndexableList<IDocumentLine> Lines { get; }

        IDocumentLine GetLineByNumber(int lineNumber);

        int LineCount { get; }

        string GetText(int offset, int length);

        char GetCharAt(int offset);

        IDisposable RunUpdate();

        event EventHandler<DocumentChangeEventArgs> Changed;
    }

    public interface IEditor2
    {
        int Offset { get; set; }

        int Line { get; set; }

        int Column { get; set; }

        ILanguageService LanguageService { get; }

        ISourceFile SourceFile { get; }

        ITextDocument Document { get; }
    }

    public interface IEditor : IDisposable
    {
        int CaretOffset { get; set; }

        int Line { get; set; }

        int Column { get; set; }

        ILanguageService LanguageService { get; }

        ISourceFile SourceFile { get; }

        ITextDocument Document { get; }

        void IndentLine(int line);

        void FormatAll();

        void Focus();

        void TriggerCodeAnalysis();

        void Save();

        void Comment();

        void Uncomment();

        void Undo();

        void Redo();

        void SetDebugHighlight(int line, int startColumn, int endColumn);

        void ClearDebugHighlight();

        void GotoOffset(int offset);

        void GotoPosition(int line, int column);

        void RenameSymbol(int offset);

        event EventHandler<TooltipDataRequestEventArgs> RequestTooltipContent;

        /// <summary>
        /// Occurs when the TextArea receives text input.
        /// but occurs immediately before the TextArea handles the TextInput event.
        /// </summary>
        event EventHandler<TextInputEventArgs> TextEntering;

        /// <summary>
        /// Occurs when the TextArea receives text input.
        /// but occurs immediately after the TextArea handles the TextInput event.
        /// </summary>
        event EventHandler<TextInputEventArgs> TextEntered;

        event EventHandler LostFocus;
    }
}
