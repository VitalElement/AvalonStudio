namespace AvalonStudio.TextEditor.Rendering
{
    using System;
    using Avalonia;
    using Avalonia.Media;

    public class SelectedDebugLineBackgroundRenderer : IBackgroundRenderer
    {
        private IBrush selectedLineBg;

        public SelectedDebugLineBackgroundRenderer()
        {            
            selectedLineBg = Brush.Parse("#44008299");
        }

        private int line;

        public event EventHandler<EventArgs> DataChanged;

        public int Line
        {
            get { return line; }
            set
            {
                line = value;

                if(DataChanged != null)
                {
                    DataChanged(this, new EventArgs());
                }
            }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (line > 0 && line < textView.TextDocument.LineCount)
            {
                var currentLine = textView.TextDocument.GetLineByNumber(line);

                var rects = VisualLineGeometryBuilder.GetRectsForSegment(textView, currentLine);

                foreach (var rect in rects)
                {
                    var drawRect = new Rect(rect.TopLeft.X, rect.TopLeft.Y, textView.Bounds.Width, rect.Height);
                    drawingContext.FillRectangle(selectedLineBg, drawRect);
                }
            }            
        }

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
            
        }
    }
}
