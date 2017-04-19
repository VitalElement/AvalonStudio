using AvalonStudio.TextEditor.Document;
using System;

namespace AvalonStudio.TextEditor.Indentation.CSharp
{
    /// <summary>
    ///     Interface used for the indentation class to access the document.
    /// </summary>
    public interface IDocumentAccessor
    {
        /// <summary>
        ///     Gets if the current line is read only (because it is not in the
        ///     selected text region)
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>Gets the number of the current line.</summary>
        int LineNumber { get; }

        /// <summary>Gets/Sets the text of the current line.</summary>
        string Text { get; set; }

        /// <summary>Advances to the next line.</summary>
        bool MoveNext();
    }

    #region TextDocumentAccessor

    /// <summary>
    ///     Adapter IDocumentAccessor -> TextDocument
    /// </summary>
    public sealed class TextDocumentAccessor : IDocumentAccessor
    {
        private readonly TextDocument doc;
        private readonly int maxLine;
        private readonly int minLine;
        private DocumentLine line;

        private bool lineDirty;

        private string text;

        /// <summary>
        ///     Creates a new TextDocumentAccessor.
        /// </summary>
        public TextDocumentAccessor(TextDocument document)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            doc = document;
            minLine = 1;
            maxLine = doc.LineCount;
        }

        /// <summary>
        ///     Creates a new TextDocumentAccessor that indents only a part of the document.
        /// </summary>
        public TextDocumentAccessor(TextDocument document, int minLine, int maxLine)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            doc = document;
            this.minLine = minLine;
            this.maxLine = maxLine;
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return LineNumber < minLine; }
        }

        /// <inheritdoc />
        public int LineNumber { get; private set; }

        /// <inheritdoc />
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (LineNumber < minLine) return;
                text = value;
                lineDirty = true;
            }
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            if (lineDirty)
            {
                doc.Replace(line, text);
                lineDirty = false;
            }
            ++LineNumber;
            if (LineNumber > maxLine) return false;
            line = doc.GetLineByNumber(LineNumber);
            text = doc.GetText(line);
            return true;
        }
    }

    #endregion TextDocumentAccessor
}