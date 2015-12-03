namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex;
    using Perspex.Media;
    using System.Collections.Generic;

    public class SelectedLineBackgroundRenderer : IBackgroundRenderer
    {
        private TextEditor editor;

        public SelectedLineBackgroundRenderer(TextEditor editor)
        {
            this.editor = editor;
        }

        static IEnumerable<Rect> GetRectsForSegmentImpl(TextView textView, ISegment segment, bool extendToFullWidthAtLineEnd = false)
        {
            int segmentStart = segment.Offset;
            int segmentEnd = segment.Offset + segment.Length;

            TextViewPosition start = new TextViewPosition(textView.TextDocument.GetLocation(segmentStart));
            TextViewPosition end = new TextViewPosition (textView.TextDocument.GetLocation(segmentEnd));

            foreach(var line in textView.TextDocument.Lines)
            {
                if(line.Offset > segmentEnd)
                {
                    break;
                }

                if(line.EndOffset < segmentStart)
                {
                    continue;
                }

                int visualColumnStart;

                if(segmentStart < line.Offset)
                {
                    visualColumnStart = 0;
                }
                else
                {
                    visualColumnStart = start.Column;
                }

                int visualColumnEnd;

                if(segmentEnd > line.EndOffset)
                {
                    // Here for later when we cope with variable char sizes.
                    visualColumnEnd = end.Column;
                }
                else
                {
                    visualColumnEnd = end.Column;
                }

                yield return new Rect(textView.CharSize.Width * (start.Column-1), textView.CharSize.Height * (start.Line - 1), (visualColumnEnd - visualColumnStart) * textView.CharSize.Width, textView.CharSize.Height);
            }
            
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            var currentLine = editor.TextDocument.GetLineByOffset(editor.CaretIndex);

            var rects = GetRectsForSegmentImpl(textView, currentLine);

            foreach(var rect in rects)
            {
                drawingContext.DrawRectangle(new Pen(Brushes.Red), rect);
            }
            
        }
    }
}
