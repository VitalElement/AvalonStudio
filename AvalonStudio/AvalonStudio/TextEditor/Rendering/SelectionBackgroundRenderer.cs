namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex.Media;

    class SelectionBackgroundRenderer : IBackgroundRenderer
    {
        public SelectionBackgroundRenderer()
        {
            selectionBrush = Brush.Parse("#AA569CD6");
        }

        private Brush selectionBrush;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {            
            if (textView.SelectionStart != textView.SelectionEnd)
            {
                TextSegment selection;

                if (textView.SelectionEnd > textView.SelectionStart)
                {
                    selection = new TextSegment { StartOffset = textView.SelectionStart, EndOffset = textView.SelectionEnd };
                }
                else
                {
                    selection = new TextSegment { StartOffset = textView.SelectionEnd, EndOffset = textView.SelectionStart };
                }

                var rects = VisualLineGeometryBuilder.GetRectsForSegment(textView, selection);

                foreach (var rect in rects)
                {
                    drawingContext.FillRectangle(selectionBrush, rect);
                }
            }
        }
    }
}
