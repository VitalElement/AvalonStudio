using AvalonStudio.TextEditor.Document;
using System;

namespace AvalonStudio.TextEditor.Indentation
{
    /// <summary>
    ///     Handles indentation by copying the indentation from the previous line.
    ///     Does not support indenting multiple lines.
    /// </summary>
    public class DefaultIndentationStrategy : IIndentationStrategy
    {
        /// <inheritdoc />
        public virtual int IndentLine(TextDocument document, DocumentLine line, int caretOffset)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (line == null)
                throw new ArgumentNullException("line");

            var previousLine = line.PreviousLine;
            if (previousLine != null)
            {
                var indentationSegment = TextUtilities.GetWhitespaceAfter(document, previousLine.Offset);
                var indentation = document.GetText(indentationSegment);
                // copy indentation to line
                indentationSegment = TextUtilities.GetWhitespaceAfter(document, line.Offset);
                document.Replace(indentationSegment, indentation);
            }

            return caretOffset;
        }

        /// <summary>
        ///     Does nothing: indenting multiple lines is useless without a smart indentation strategy.
        /// </summary>
        public virtual int IndentLines(TextDocument document, int beginLine, int endLine, int caretOffset)
        {
            return caretOffset;
        }
    }
}