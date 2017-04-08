using System;
using Avalonia;
using Avalonia.Media;

namespace AvalonStudio.TextEditor.Rendering
{
	public class SelectedDebugLineBackgroundRenderer : IBackgroundRenderer
	{
		private int line;
		private readonly IBrush selectedLineBg;

		public SelectedDebugLineBackgroundRenderer()
		{
			selectedLineBg = Brush.Parse("#C5C870");
		}

		public int Line
		{
			get { return line; }
			set
			{
				line = value;

				if (DataChanged != null)
				{
					DataChanged(this, new EventArgs());
				}
			}
		}

		public event EventHandler<EventArgs> DataChanged;

		public void Draw(TextView textView, DrawingContext drawingContext)
		{
			if (line > 0 && line < textView.TextDocument.LineCount)
			{
				var currentLine = textView.TextDocument.GetLineByNumber(line);                                  

				var rects = VisualLineGeometryBuilder.GetRectsForSegment(textView, currentLine);

				foreach (var rect in rects)
				{
					var drawRect = new Rect(rect.TopLeft.X - 1, rect.TopLeft.Y - 1, rect.Width + 2, rect.Height + 2);
					drawingContext.FillRectangle(selectedLineBg, drawRect);
				}
			}
		}

		public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
		{
            if (line.DocumentLine.LineNumber == Line)
            {
                line.RenderedText.SetForegroundBrush(Brushes.Black, 0, line.Length);
            }
		}
	}
}