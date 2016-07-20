using System;
using AvalonStudio.TextEditor.Document;

namespace AvalonStudio.TextEditor.Indentation.CSharp
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
		private string indentationString = "    ";

		/// <summary>
		///     Gets/Sets the indentation string.
		/// </summary>
		public string IndentationString
		{
			get { return indentationString; }
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
		public int Indent(IDocumentAccessor document, bool keepEmptyLines, int caretOffset)
		{
			if (document == null)
				throw new ArgumentNullException("document");
			var settings = new IndentationSettings();
			settings.IndentString = IndentationString;
			settings.LeaveEmptyLines = keepEmptyLines;

			var r = new IndentationReformatter();
			r.Reformat(document, settings);
			return caretOffset;
		}

		/// <inheritdoc cref="IIndentationStrategy.IndentLine" />
		public override int IndentLine(TextDocument document, DocumentLine line, int caretOffset)
		{
			var lineNr = line.LineNumber;
			var acc = new TextDocumentAccessor(document, lineNr, lineNr);
			var result = Indent(acc, false, caretOffset);

			var t = acc.Text;
			if (t.Length == 0)
			{
				// use AutoIndentation for new lines in comments / verbatim strings.
				return base.IndentLine(document, line, caretOffset);
			}

			return result;
		}

		/// <inheritdoc cref="IIndentationStrategy.IndentLines" />
		public override int IndentLines(TextDocument document, int beginLine, int endLine, int caretOffset)
		{
			return Indent(new TextDocumentAccessor(document, beginLine, endLine), true, caretOffset);
		}
	}
}