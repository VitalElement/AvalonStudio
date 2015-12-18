namespace AvalonStudio.TextEditor.Rendering
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Perspex.Media;

    class ColumnLimitBackgroundRenderer : IBackgroundRenderer
    {
        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            double xPos = textView.TextSurfaceBounds.X + textView.CharSize.Width * 80.0;

            drawingContext.DrawLine(new Pen(Brushes.LightGray, 1), new Perspex.Point(xPos, 0), new Perspex.Point(xPos, textView.Bounds.Bottom));
        }
    }
}
