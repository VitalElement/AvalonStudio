namespace AvalonStudio.TextEditor.Rendering
{
    using System;
    using Perspex.Media;

    public class ColumnLimitBackgroundRenderer : IBackgroundRenderer
    {
        private Brush brush = Brush.Parse("#30E4E4E4");

        public event EventHandler<EventArgs> DataChanged;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            double xPos = textView.TextSurfaceBounds.X + textView.CharSize.Width * 80.0;

            drawingContext.DrawLine(new Pen(brush, 1), new Perspex.Point(xPos, 0), new Perspex.Point(xPos, textView.Bounds.Bottom));
        }
    }
}
