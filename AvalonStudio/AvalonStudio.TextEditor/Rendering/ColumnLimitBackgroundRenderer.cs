using System;
using Avalonia;
using Avalonia.Media;

namespace AvalonStudio.TextEditor.Rendering
{
	public class ColumnLimitBackgroundRenderer : IBackgroundRenderer
	{
		private readonly IBrush brush = Brush.Parse("#30E4E4E4");

        public event EventHandler<EventArgs> DataChanged;

		public void Draw(TextView textView, DrawingContext drawingContext)
		{
			var xPos = textView.TextSurfaceBounds.X + textView.CharSize.Width*120.0;

			drawingContext.DrawLine(new Pen(brush, 1), new Point(xPos, 0), new Point(xPos, textView.Bounds.Bottom));
		}

		public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
		{
		}
	}
}