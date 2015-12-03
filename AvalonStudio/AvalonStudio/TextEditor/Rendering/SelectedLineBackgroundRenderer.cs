namespace AvalonStudio.TextEditor.Rendering
{
    using Perspex;
    using Perspex.Media;

    public class SelectedLineBackgroundRenderer : IBackgroundRenderer
    {
        private TextEditor editor;
        private Brush selectedLineBg;

        public SelectedLineBackgroundRenderer(TextEditor editor)
        {
            selectedLineBg = Brush.Parse("#FF0E0E0E");
            this.editor = editor;
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (editor.CaretIndex != -1)
            {
                var currentLine = editor.TextDocument.GetLineByOffset(editor.CaretIndex);

                var rects = VisualLineGeometryBuilder.GetRectsForSegment(textView, currentLine);

                foreach (var rect in rects)
                {
                    var drawRect = new Rect(rect.TopLeft.X + 4, rect.TopLeft.Y, textView.Bounds.Width, rect.Height);
                    drawingContext.FillRectangle(selectedLineBg, drawRect);
                }
            }            
        }
    }
}
