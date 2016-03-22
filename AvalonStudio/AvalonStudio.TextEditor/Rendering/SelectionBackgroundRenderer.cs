namespace AvalonStudio.TextEditor.Rendering
{
    using System;
    using Document;
    using Perspex.Media;

    public class SelectionBackgroundRenderer : IBackgroundRenderer
    {
        public SelectionBackgroundRenderer()
        {
            selectionBrush = Brush.Parse("#AA569CD6");
        }

        private Brush selectionBrush;

        public event EventHandler<EventArgs> DataChanged;

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

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
            
        }
    }
}
