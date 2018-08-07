using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvaloniaEdit.Utils;
using System;

namespace AvalonStudio.TextEditor.Rendering
{
    public class SelectedLineBackgroundRenderer : IBackgroundRenderer
    {
        private readonly AvaloniaEdit.TextEditor _textEditor;

        public static readonly IBrush BackgroundBrush = new SolidColorBrush(Color.FromArgb(22, 0x0e, 0x0e, 0x0e));
        public static readonly IBrush BorderBrush = new SolidColorBrush(Color.FromArgb(255, 57, 57, 57));

        public Pen BorderPen { get; set; }

        public SelectedLineBackgroundRenderer(AvaloniaEdit.TextEditor textEditor)
        {
            _textEditor = textEditor;

            BorderPen = new Pen(BackgroundBrush);
        }

        public KnownLayer Layer => KnownLayer.Background;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (_textEditor.Document != null)
            {
                if (_textEditor.SelectionLength == 0 && _textEditor.CaretOffset != -1 &&
                    _textEditor.CaretOffset <= textView.Document.TextLength)
                {
                    var currentLine = textView.Document.GetLocation(_textEditor.CaretOffset).Line;

                    var visualLine = textView.GetVisualLine(currentLine);
                    if (visualLine == null) return;

                    BackgroundGeometryBuilder builder = new BackgroundGeometryBuilder();

                    var linePosY = visualLine.VisualTop - textView.ScrollOffset.Y;
                    var lineBottom = linePosY + visualLine.Height;

                    Size pixelSize = PixelSnapHelpers.GetPixelSize(textView);


                    double x = PixelSnapHelpers.PixelAlign(0, pixelSize.Width);
                    double y = PixelSnapHelpers.PixelAlign(linePosY, pixelSize.Height);
                    var x2 = PixelSnapHelpers.PixelAlign(textView.Bounds.Width - pixelSize.Width, pixelSize.Width);
                    var y2 = PixelSnapHelpers.PixelAlign(lineBottom, pixelSize.Height);

                    builder.AddRectangle(textView, new Rect(new Point(x, y), new Point(x2, y2)));

                    Geometry geometry = builder.CreateGeometry();
                    if (geometry != null)
                    {
                        drawingContext.DrawGeometry(BackgroundBrush, BorderPen, geometry);
                    }
                }
            }
        }
    }
}