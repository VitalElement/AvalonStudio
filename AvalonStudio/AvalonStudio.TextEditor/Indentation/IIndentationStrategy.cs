using AvalonStudio.TextEditor.Document;

namespace AvalonStudio.TextEditor.Indentation
{
    /// <summary>
    ///     Strategy how the text editor handles indentation when new lines are inserted.
    /// </summary>
    public interface IIndentationStrategy
    {
        /// <summary>
        ///     Sets the indentation for the specified line.
        ///     Usually this is constructed from the indentation of the previous line.
        /// </summary>
        int IndentLine(TextDocument document, DocumentLine line, int caretOffset);

        /// <summary>
        ///     Reindents a set of lines.
        /// </summary>
        int IndentLines(TextDocument document, int beginLine, int endLine, int caretOffset);
    }
}