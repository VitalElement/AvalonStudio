namespace AvalonStudio.TextEditor.Rendering
{
    using System;
    using Avalonia.Media;

    public class ColumnLimitBackgroundRenderer : IBackgroundRenderer
    {
        private IBrush brush = Brush.Parse("#30E4E4E4");

        public event EventHandler<EventArgs> DataChanged;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            double xPos = textView.TextSurfaceBounds.X + textView.CharSize.Width * 120.0;

            drawingContext.DrawLine(new Pen(brush, 1), new Avalonia.Point(xPos, 0), new Avalonia.Point(xPos, textView.Bounds.Bottom));
        }

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
            
        }
    }
}
