namespace AvalonStudio.TextEditor.Rendering
{
    using Perspex;
    using Perspex.Media;

    public class SelectedLineBackgroundRenderer : IBackgroundRenderer
    {
        private Brush selectedLineBg;

        public SelectedLineBackgroundRenderer()
        {
            selectedLineBg = Brush.Parse("#FF0E0E0E");
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (textView.CaretIndex != -1 && textView.CaretIndex < textView.TextDocument.TextLength)
            {
                var currentLine = textView.TextDocument.GetLineByOffset(textView.CaretIndex);

                var rects = VisualLineGeometryBuilder.GetRectsForSegment(textView, currentLine);

                foreach (var rect in rects)
                {
                    var drawRect = new Rect(rect.TopLeft.X, rect.TopLeft.Y, textView.Bounds.Width, rect.Height);
                    drawingContext.FillRectangle(selectedLineBg, drawRect);
                }
            }            
        }
    }
}
