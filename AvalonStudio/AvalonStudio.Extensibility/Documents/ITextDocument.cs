using Avalonia.Input;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
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

    public interface ITextDocument
    {
        void Insert(int offset, string text);

        void Replace(int offset, int length, string text);

        string Text { get; }

        int TextLength { get; }

        IIndexableList<IDocumentLine> Lines { get; }

        IDocumentLine GetLineByNumber(int lineNumber);

        int LineCount { get; }

        string GetText(int offset, int length);

        char GetCharAt(int offset);

        IDisposable RunUpdate();
    }

    public interface IEditor : IDisposable
    {
        int Offset { get; set; }

        int Line { get; set; }

        int Column { get; set; }

        ISourceFile SourceFile { get; }

        ITextDocument Document { get; }

        void IndentLine(int line);

        void FormatAll();

        void Focus();

        void TriggerCodeAnalysis();

        void Save();

        void Comment();

        void UnComment();

        void Undo();

        void Redo();

        void SetDebugHighlight(int line, int startColumn, int endColumn);

        void ClearDebugHighlight();

        void GotoOffset(int offset);

        void GotoPosition(int line, int column);

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
    }
}
