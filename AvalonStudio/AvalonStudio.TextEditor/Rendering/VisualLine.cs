using Avalonia.Media;
using AvalonStudio.TextEditor.Document;

namespace AvalonStudio.TextEditor.Rendering
{
	public class VisualLine : ISegment
	{
		public DocumentLine DocumentLine { get; set; }
		public uint VisualLineNumber { get; set; }
		public FormattedText RenderedText { get; set; }

		public int Offset
		{
			get { return DocumentLine.Offset; }
		}

		public int Length
		{
			get { return DocumentLine.Length; }
		}

		public int EndOffset
		{
			get { return DocumentLine.EndOffset; }
		}

		~VisualLine()
		{
			RenderedText?.Dispose();
			RenderedText = null;
			DocumentLine = null;
		}
	}
}