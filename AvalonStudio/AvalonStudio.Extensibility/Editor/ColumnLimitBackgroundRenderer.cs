using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Rendering;
using System;

namespace AvalonStudio.TextEditor.Rendering
{
    public class ColumnLimitBackgroundRenderer : IBackgroundRenderer
    {
        private readonly IBrush brush = Brush.Parse("#30E4E4E4");

        public KnownLayer Layer => KnownLayer.Background;

        public event EventHandler<EventArgs> DataChanged;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
           /* var horizontalPosition = textView.TextSurfaceBounds.X + (textView.CharSize.Width * 120.0);

            drawingContext.DrawLine(new Pen(brush, 1), new Point(horizontalPosition, 0), new Point(horizontalPosition, textView.Bounds.Bottom));*/
        }

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
        }
    }
}