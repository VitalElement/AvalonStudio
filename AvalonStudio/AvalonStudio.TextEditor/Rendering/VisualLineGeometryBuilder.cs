namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using System;
    using System.Collections.Generic;

    public class VisualLineGeometryBuilder
    {
        public static Rect GetTextViewPosition(TextView textView, int offset)
        {
            var position = new TextViewPosition(textView.GetLocation(offset));

            return GetTextPosition(textView, position);
        }

        public static Rect GetDocumentTextPosition(TextView textView, int offset)
        {
            var position = new TextViewPosition(textView.TextDocument.GetLocation(offset));

            return GetTextPosition(textView, position);
        }

        public static Rect GetTextPosition(TextView textView, TextViewPosition position)
        {
            return new Rect(textView.TextSurfaceBounds.X + textView.CharSize.Width * (position.Column - 1), textView.CharSize.Height * (position.Line - 1), textView.CharSize.Width, textView.CharSize.Height);
        }

        private static IEnumerable<Tuple<int, int>> GetOffsetForLinesInSegmentOnScreen(TextView textView, ISegment segment, bool extendToFullWidthAtLineEnd = false)
        {
            int segmentStart = segment.Offset;
            int segmentEnd = segment.Offset + segment.Length;

            if (segmentStart > textView.TextDocument.TextLength)
            {
                segmentStart = textView.TextDocument.TextLength;
            }

            if (segmentEnd > textView.TextDocument.TextLength)
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
                    yield return new Tuple<int, int>(lineStartOffset, lineEndOffset);
                }
            }
        }

        private static IEnumerable<Tuple<int, int>> GetOffsetForLinesInSegmentInDocument(TextDocument textDocument, ISegment segment, bool extendToFullWidthAtLineEnd = false)
        {
            int segmentStart = segment.Offset;
            int segmentEnd = segment.Offset + segment.Length;

            if (segmentStart > textDocument.TextLength)
            {
                segmentStart = textDocument.TextLength;
            }

            if (segmentEnd > textDocument.TextLength)
            {
                segmentEnd = textDocument.TextLength;
            }

            TextViewPosition start = new TextViewPosition(textDocument.GetLocation(segmentStart));
            TextViewPosition end = new TextViewPosition(textDocument.GetLocation(segmentEnd));

            foreach (var line in textDocument.Lines)
            {
                if (!line.IsDeleted)
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
                    yield return new Tuple<int, int>(lineStartOffset, lineEndOffset);
                }
            }
        }

        public static IEnumerable<IDocumentLine> GetLinesForSegmentInDocument(TextDocument textDocument, ISegment segment, bool extendToFullWidthAtLineEnd = false)
        {
            foreach (var tuple in GetOffsetForLinesInSegmentInDocument(textDocument, segment, extendToFullWidthAtLineEnd))
            {
                yield return textDocument.GetLineByOffset(tuple.Item1);
            }
        }

        public static IEnumerable<IDocumentLine> GetLinesForSegmentOnScreen(TextView textView, ISegment segment, bool extendToFullWidthAtLineEnd = false)
        {
            foreach (var tuple in GetOffsetForLinesInSegmentOnScreen(textView, segment, extendToFullWidthAtLineEnd))
            {
                yield return textView.TextDocument.GetLineByOffset(tuple.Item1);
            }
        }

        public static IEnumerable<Rect> GetRectsForSegment(TextView textView, ISegment segment, bool extendToFullWidthAtLineEnd = false)
        {
            foreach (var tuple in GetOffsetForLinesInSegmentOnScreen(textView, segment, extendToFullWidthAtLineEnd))
            {
                yield return new Rect(GetTextViewPosition(textView, tuple.Item1).TopLeft, GetTextViewPosition(textView, tuple.Item2).BottomLeft);
            }
        }
    }
}
