using System;
using System.IO;
using System.Text;

namespace AvalonStudio.TextEditor.Document
{
	/// <summary>
	///     A TextWriter implementation that directly inserts into a document.
	/// </summary>
	public class DocumentTextWriter : TextWriter
	{
		private readonly IDocument document;

		/// <summary>
		///     Creates a new DocumentTextWriter that inserts into document, starting at insertionOffset.
		/// </summary>
		public DocumentTextWriter(IDocument document, int insertionOffset)
		{
			InsertionOffset = insertionOffset;
			if (document == null)
				throw new ArgumentNullException("document");
			this.document = document;
			var line = document.GetLineByOffset(insertionOffset);
			if (line.DelimiterLength == 0)
				line = line.PreviousLine;
			if (line != null)
				NewLine = document.GetText(line.EndOffset, line.DelimiterLength);
		}

		/// <summary>
		///     Gets/Sets the current insertion offset.
		/// </summary>
		public int InsertionOffset { get; set; }

		/// <inheritdoc />
		public override Encoding Encoding
		{
			get { return Encoding.UTF8; }
		}

		/// <inheritdoc />
		public override void Write(char value)
		{
			document.Insert(InsertionOffset, value.ToString());
			InsertionOffset++;
		}

		/// <inheritdoc />
		public override void Write(char[] buffer, int index, int count)
		{
			document.Insert(InsertionOffset, new string(buffer, index, count));
			InsertionOffset += count;
		}

		/// <inheritdoc />
		public override void Write(string value)
		{
			document.Insert(InsertionOffset, value);
			InsertionOffset += value.Length;
		}
	}
}