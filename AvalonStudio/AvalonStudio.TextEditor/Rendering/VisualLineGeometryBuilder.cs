namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using System.Collections.Generic;

    public class VisualLineGeometryBuilder
    {
        public static Rect GetTextViewPosition (TextView textView, int offset)
        {
            var position = new TextViewPosition(textView.GetLocation(offset));            

            return GetTextPosition(textView, position);
        }

        public static Rect GetDocumentTextPosition (TextView textView, int offset)
        {
            var position = new TextViewPosition(textView.TextDocument.GetLocation(offset));

            return GetTextPosition(textView, position);
        }

        public static Rect GetTextPosition (TextView textView, TextViewPosition position)
        {
            return new Rect(textView.TextSurfaceBounds.X + textView.CharSize.Width * (position.Column - 1), textView.CharSize.Height * (position.Line - 1), textView.CharSize.Width, textView.CharSize.Height);
        }

        public static IEnumerable<Rect> GetRectsForSegment(TextView textView, ISegment segment, bool extendToFullWidthAtLineEnd = false)
        {
            int segmentStart = segment.Offset;
            int segmentEnd = segment.Offset + segment.Length;

            if(segmentEnd > textView.TextDocument.TextLength)
            {
                segmentEnd = textView.TextDocument.TextLength;
            }

            TextViewPosition start = new TextViewPosition(textView.TextDocument.GetLocation(segmentStart));
            TextViewPosition end = new TextViewPosition(textView.TextDocument.GetLocation(segmentEnd));

            foreach (var line in textView.VisualLines)
            {
                if (!line.DocumentLine.IsDeleted)
                {
                    if (line.Offset > segmentEnd)
                    {
                        break;
                    }

                    if (line.EndOffset < segmentStart)
                    {
                        continue;
                    }

                    // find start and begining in current line.
                    var lineStartOffset = line.Offset;

                    if (segment.Offset > line.Offset)
                    {
                        lineStartOffset = line.Offset + (segment.Offset - line.Offset);
                    }

                    var lineEndOffset = line.EndOffset;

                    if (segment.EndOffset < line.EndOffset)
                    {
                        lineEndOffset = line.EndOffset - (line.EndOffset - segment.EndOffset);
                    }

                    // generate rect for section in this line.
                    yield return new Rect(GetTextViewPosition(textView, lineStartOffset).TopLeft, GetTextViewPosition(textView, lineEndOffset).BottomLeft);
                }
            }

        }
    }
}
