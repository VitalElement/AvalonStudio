using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Indentation;
using AvalonStudio.TextEditor.Indentation.CSharp;
using System;

namespace AvalonStudio.Languages.CSharp
{
    /// <summary>
    ///     Smart indentation for C#.
    /// </summary>
    public class CSharpIndentationStrategy : DefaultIndentationStrategy
    {
        /// <summary>
        ///     Creates a new CSharpIndentationStrategy and initializes the settings using the text editor options.
        /// </summary>
        //public CSharpIndentationStrategy(TextEditorOptions options)
        //{
        //    this.IndentationString = options.IndentationString;
        //}
        private string indentationString = new string(' ', 4);

        /// <summary>
        ///     Gets/Sets the indentation string.
        /// </summary>
        public string IndentationString
        {
            get
            {
                return indentationString;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("Indentation string must not be null or empty");
                indentationString = value;
            }
        }

        /// <summary>
        ///     Performs indentation using the specified document accessor.
        /// </summary>
        /// <param name="document">Object used for accessing the document line-by-line</param>
        /// <param name="keepEmptyLines">Specifies whether empty lines should be kept</param>
        public int Indent(IDocumentAccessor document, bool keepEmptyLines, int caretIndex)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));
            var settings = new IndentationSettings();
            settings.IndentString = IndentationString;
            settings.LeaveEmptyLines = keepEmptyLines;

            var r = new IndentationReformatter();
            r.Reformat(document, settings);

            return caretIndex;
        }

        /// <inheritdoc cref="IIndentationStrategy.IndentLine" />
        public override int IndentLine(TextDocument document, DocumentLine line, int caretIndex)
        {
            if (line == null)
            {
                return caretIndex;
            }

            var lineNr = line.LineNumber;
            var acc = new TextDocumentAccessor(document, lineNr, lineNr);

            var leadingWhiteSpaceBefore = TextUtilities.GetLeadingWhitespace(document, line).Length;
            var result = Indent(acc, false, caretIndex);
            var t = acc.Text;

            result = caretIndex + TextUtilities.GetLeadingWhitespace(document, line).Length - leadingWhiteSpaceBefore;

            if (t.Length == 0)
            {
                // use AutoIndentation for new lines in comments / verbatim strings.
                return base.IndentLine(document, line, caretIndex);
            }

            return result;
        }

        /// <inheritdoc cref="IIndentationStrategy.IndentLines" />
        public override int IndentLines(TextDocument document, int beginLine, int endLine, int caretIndex)
        {
            return Indent(new TextDocumentAccessor(document, beginLine, endLine), true, caretIndex);
        }
    }
}