using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Rendering;
using AvaloniaEdit.Utils;
using System;

namespace AvalonStudio.TextEditor.Rendering
{
    public class ColumnLimitBackgroundRenderer : IBackgroundRenderer
    {
        private readonly IBrush brush = Brush.Parse("#30E4E4E4");
        private readonly Pen _pen;

        public ColumnLimitBackgroundRenderer()
        {
            _pen = new Pen(brush);
        }

        public KnownLayer Layer => KnownLayer.Background;

        private int _column = 120;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            double offset = textView.WideSpaceWidth * _column;
            Size pixelSize = PixelSnapHelpers.GetPixelSize(textView);
            double markerXPos = PixelSnapHelpers.PixelAlign(offset, pixelSize.Width);
            markerXPos -= textView.ScrollOffset.X;
            Point start = new Point(markerXPos, 0);
            Point end = new Point(markerXPos, Math.Max(textView.DocumentHeight, textView.Bounds.Height));

            drawingContext.DrawLine(_pen, start, end);
        }
    }
}