using Avalonia.Input;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace AvalonStudio.Documents
{
    public static class ITextEditorExtensions
    {
        public static IDocumentLine CurrentLine(this ITextEditor editor) => 
            editor.Line > 0 && editor.Line <= editor.Document.LineCount ? 
            editor.Document.Lines[editor.Line] : null;

        public static IDocumentLine PreviousLine(this ITextEditor editor) =>
            editor.CurrentLine()?.PreviousLine;

        public static string CurrentLineText(this ITextEditor editor) => editor.Document.GetText(editor.CurrentLine());

        public static string PreviousLineText(this ITextEditor editor) => editor.Document.GetText(editor.PreviousLine());
    }

    public static class ITextDocumentExtensions
    {
        public static void TrimTrailingWhiteSpace(this ITextDocument document, ISegment line)
        {
            var lineText = document.GetText(line);

            var index = lineText.Length;

            while (index > 0)
            {
                if (!char.IsWhiteSpace(lineText[index - 1]))
                {
                    break;
                }

                index--;
            }

            if (index != lineText.Length)
            {
                document.Replace(line.Offset + index, lineText.Length - index, "");
            }
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

        public static ISegment GetToken(this ITextDocument document, int offset)
        {
            var result = string.Empty;

            if (offset >= 0 && document.TextLength > offset)
            {
                var start = offset;

                var currentChar = document.GetCharAt(offset);
                var prevChar = '\0';

                if (offset > 0)
                {
                    prevChar = document.GetCharAt(offset - 1);
                }

                var charClass = AvaloniaEdit.Document.TextUtilities.GetCharacterClass(currentChar);

                if (charClass != AvaloniaEdit.Document.CharacterClass.LineTerminator && prevChar != ' ' &&
                    AvaloniaEdit.Document.TextUtilities.GetCharacterClass(prevChar) != AvaloniaEdit.Document.CharacterClass.LineTerminator)
                {
                    start = TextUtilities.GetNextCaretPosition(document, offset, LogicalDirection.Backward,
                        CaretPositioningMode.WordStart);
                }

                var end = TextUtilities.GetNextCaretPosition(document, start, LogicalDirection.Forward,
                    CaretPositioningMode.WordBorder);

                if (start >= 0 && end >= 0 && end < document.TextLength && end >= start)
                {
                    return new SimpleSegment(start, end - start);
                }
            }

            return null;
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

        public static (int index, char character) GetLastNonWhiteSpaceCharBefore(this ITextDocument textSource, int index, int minIndex = 0)
        {
            while(index >= minIndex)
            {
                var currentChar = textSource.GetCharAt(index);

                if (!char.IsWhiteSpace(currentChar))
                {
                    return (index, currentChar);
                }

                index--;
            }

            return (-1, '\0');
        }

        public static (int index, char character) GetLastCharMatching (this ITextDocument textSource, Predicate<char> predicate, int startIndex, int minIndex = 0, int skip = 0)
        {
            while (startIndex >= minIndex)
            {
                var currentChar = textSource.GetCharAt(startIndex);

                if(predicate(currentChar))
                {
                    if (skip == 0)
                    {
                        return (startIndex, currentChar);
                    }
                    else
                    {
                        skip--;
                    }
                }

                startIndex--;
            }

            return (-1, '\0');
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


    /// !!! NOTE This enum must match the one in AvalonEdit !!!
    /// <summary>
	/// Contains predefined offset change mapping types.
	/// </summary>
	public enum ReplaceMode
    {
        /// <summary>
        /// Normal replace.
        /// Anchors in front of the replaced region will stay in front, anchors after the replaced region will stay after.
        /// Anchors in the middle of the removed region will be deleted. If they survive deletion,
        /// they move depending on their AnchorMovementType.
        /// </summary>
        /// <remarks>
        /// This is the default implementation of DocumentChangeEventArgs when OffsetChangeMap is null,
        /// so using this option usually works without creating an OffsetChangeMap instance.
        /// This is equivalent to an OffsetChangeMap with a single entry describing the replace operation.
        /// </remarks>
        Normal,
        /// <summary>
        /// First the old text is removed, then the new text is inserted.
        /// Anchors immediately in front (or after) the replaced region may move to the other side of the insertion,
        /// depending on the AnchorMovementType.
        /// </summary>
        /// <remarks>
        /// This is implemented as an OffsetChangeMap with two entries: the removal, and the insertion.
        /// </remarks>
        RemoveAndInsert,
        /// <summary>
        /// The text is replaced character-by-character.
        /// Anchors keep their position inside the replaced text.
        /// Anchors after the replaced region will move accordingly if the replacement text has a different length than the replaced text.
        /// If the new text is shorter than the old text, anchors inside the old text that would end up behind the replacement text
        /// will be moved so that they point to the end of the replacement text.
        /// </summary>
        /// <remarks>
        /// On the OffsetChangeMap level, growing text is implemented by replacing the last character in the replaced text
        /// with itself and the additional text segment. A simple insertion of the additional text would have the undesired
        /// effect of moving anchors immediately after the replaced text into the replacement text if they used
        /// AnchorMovementStyle.BeforeInsertion.
        /// Shrinking text is implemented by removing the text segment that's too long; but in a special mode that
        /// causes anchors to always survive irrespective of their <see cref="TextAnchor.SurviveDeletion"/> setting.
        /// If the text keeps its old size, this is implemented as OffsetChangeMap.Empty.
        /// </remarks>
        CharacterReplace,
        /// <summary>
        /// Like 'Normal', but anchors with <see cref="TextAnchor.MovementType"/> = Default will stay in front of the
        /// insertion instead of being moved behind it.
        /// </summary>
        KeepAnchorBeforeInsertion
    }

    public interface ITextDocument
    {
        void Insert(int offset, string text);

        void Replace(int offset, int length, string text, ReplaceMode replaceMode = ReplaceMode.Normal);

        TextLocation GetLocation(int offset);

        int GetOffset(int line, int column);

        string Text { get; }

        int TextLength { get; }

        ISegment CreateAnchoredSegment(int offset, int length);

        IIndexableList<IDocumentLine> Lines { get; }

        IDocumentLine GetLineByNumber(int lineNumber);

        int LineCount { get; }

        string GetText(int offset, int length);

        char GetCharAt(int offset);

        void Undo();

        void Redo();

        IDisposable RunUpdate();

        event EventHandler<DocumentChangeEventArgs> Changed;
    }

    public interface ITextEditor
    {
        int Offset { get; set; }

        int Line { get; set; }

        int Column { get; set; }

        void IndentLine(int lineNumber);

        ISourceFile SourceFile { get; }

        ITextDocument Document { get; }

        ISegment Selection { get; }

        bool OnBeforeTextEntered(string text);

        bool OnTextEntered(string text);

        void OnTextChanged();

        Task<object> GetToolTipContentAsync(int offset);

        event EventHandler<TooltipDataRequestEventArgs> TooltipContentRequested;
    }

    public interface ICodeEditor : ITextEditor
    {
        ILanguageService LanguageService { get; }

        ObservableCollection<(object tag, SyntaxHighlightDataList highlights)> Highlights { get; }

        ObservableCollection<(object tag, IEnumerable<Diagnostic> diagnostics)> Diagnostics { get; }

        IEnumerable<IndexEntry> CodeIndex { get; }

        void Comment();

        void Uncomment();
    }
}
