namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using System.Collections.Generic;

    public class VisualLineGeometryBuilder
    {
        public static Rect GetTextPosition (TextView textView, int offset)
        {
            var position = new TextViewPosition(textView.TextDocument.GetLocation(offset));

            return GetTextPosition(textView, position);
        }

        public static Rect GetTextPosition (TextView textView, TextViewPosition position)
        {
            return new Rect(textView.CharSize.Width * (position.Column - 1), textView.CharSize.Height * (position.Line - 1), textView.CharSize.Width, textView.CharSize.Height);
        }

        public static IEnumerable<Rect> GetRectsForSegment(TextView textView, ISegment segment, bool extendToFullWidthAtLineEnd = false)
        {
            int segmentStart = segment.Offset;
            int segmentEnd = segment.Offset + segment.Length;

            TextViewPosition start = new TextViewPosition(textView.TextDocument.GetLocation(segmentStart));
            TextViewPosition end = new TextViewPosition(textView.TextDocument.GetLocation(segmentEnd));

            foreach (var line in textView.TextDocument.Lines)
            {
                if (line.Offset > segmentEnd)
                {
                    break;
                }

                if (line.EndOffset < segmentStart)
                {
                    continue;
                }

                int visualColumnStart;

                if (segmentStart < line.Offset)
                {
                    visualColumnStart = 0;
                }
                else
                {
                    visualColumnStart = start.Column;
                }

                int visualColumnEnd;

                if (segmentEnd > line.EndOffset)
                {
                    // Here for later when we cope with variable char sizes.
                    visualColumnEnd = end.Column;
                }
                else
                {
                    visualColumnEnd = end.Column;
                }

                yield return new Rect(GetTextPosition(textView, start).TopLeft, new Size(textView.Bounds.Width, textView.CharSize.Height));
            }

        }
    }
}
