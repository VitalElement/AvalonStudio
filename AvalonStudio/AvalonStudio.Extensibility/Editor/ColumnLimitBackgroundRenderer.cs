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
        private UInt32 _column = 80;

        public ColumnLimitBackgroundRenderer()
        {
            _pen = new Pen(brush);
        }

        public UInt32 Column
        {
            get => _column;
            set => _column = value;
        }

        public KnownLayer Layer => KnownLayer.Background;        

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