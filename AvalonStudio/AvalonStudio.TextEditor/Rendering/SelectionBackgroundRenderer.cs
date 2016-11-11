using System;
using Avalonia.Media;
using AvalonStudio.TextEditor.Document;

namespace AvalonStudio.TextEditor.Rendering
{
	public class SelectionBackgroundRenderer : IBackgroundRenderer
	{
		private readonly IBrush selectionBrush;

		public SelectionBackgroundRenderer()
		{
			selectionBrush = Brush.Parse("#AA569CD6");
		}

        public event EventHandler<EventArgs> DataChanged;

        public void Draw(TextView textView, DrawingContext drawingContext)
		{
			if (textView.SelectionStart != textView.SelectionEnd && textView.SelectionEnd >= 0 && textView.SelectionStart >= 0)
			{
				TextSegment selection;
                
				if (textView.SelectionEnd > textView.SelectionStart)
				{
					selection = new TextSegment {StartOffset = textView.SelectionStart, EndOffset = textView.SelectionEnd};
				}
				else
				{
					selection = new TextSegment {StartOffset = textView.SelectionEnd, EndOffset = textView.SelectionStart};
				}

				var rects = VisualLineGeometryBuilder.GetRectsForSegment(textView, selection);

				foreach (var rect in rects)
				{
					drawingContext.FillRectangle(selectionBrush, rect);
				}
			}
		}

		public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
		{
		}
	}
}