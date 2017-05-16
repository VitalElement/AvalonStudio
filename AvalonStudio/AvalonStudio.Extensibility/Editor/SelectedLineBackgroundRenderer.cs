using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Rendering;
using System;

namespace AvalonStudio.TextEditor.Rendering
{
    public class SelectedLineBackgroundRenderer : IBackgroundRenderer
    {
        private readonly IBrush selectedLineBg;

        public SelectedLineBackgroundRenderer()
        {
            selectedLineBg = Brush.Parse("#FF0E0E0E");
        }

        public KnownLayer Layer => KnownLayer.Background;

        public event EventHandler<EventArgs> DataChanged;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            /*if (textView.SelectionStart == textView.SelectionEnd && textView.CaretIndex != -1 &&
                textView.CaretIndex <= textView.TextDocument.TextLength)
            {
                var currentLine = textView.TextDocument.GetLineByOffset(textView.CaretIndex);

                var rects = VisualLineGeometryBuilder.GetRectsForSegment(textView, currentLine, true);

                foreach (var rect in rects)
                {
                    var drawRect = new Rect(rect.TopLeft.X, rect.TopLeft.Y, textView.Bounds.Width, rect.Height);
                    drawingContext.FillRectangle(selectedLineBg, drawRect);
                }
            }*/
        }

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
        }
    }
}