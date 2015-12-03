namespace AvalonStudio.TextEditor.Rendering
{
    using Perspex.Media;

    public class SelectedLineBackgroundRenderer : IBackgroundRenderer
    {
        private TextEditor editor;

        public SelectedLineBackgroundRenderer(TextEditor editor)
        {
            this.editor = editor;
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            var currentLine = editor.TextDocument.GetLineByOffset(editor.CaretIndex);

            var rects = VisualLineGeometryBuilder.GetRectsForSegment(textView, currentLine);

            foreach(var rect in rects)
            {
                drawingContext.FillRectangle(Brushes.Black, rect);
                drawingContext.DrawRectangle(new Pen(Brushes.Red), rect);
            }
            
        }
    }
}
