namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex;
    using Perspex.Media;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TextMarkerService : IDocumentLineTransformer
    {
        private TextSegmentCollection<TextMarker> markers;

        public event EventHandler<EventArgs> DataChanged;

        public sealed class TextMarker : TextSegment
        {
            public TextMarker(int startOffset, int length)
            {
                StartOffset = startOffset;
                Length = length;
            }

            public Color? BackgroundColor { get; set; }
            public Color MarkerColor { get; set; }
            public string ToolTip { get; set; }
        }

        public TextMarkerService(TextDocument document)
        {
            markers = new TextSegmentCollection<TextMarker>(document);

        }

        public void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, VisualLine line)
        {
            if(markers == null)
            {
                return;
            }

            var markersInLine = markers.FindOverlappingSegments(line);

            foreach (TextMarker marker in markersInLine)
            {
                if(marker.Length == 0)
                {
                    var endoffset = TextUtilities.GetNextCaretPosition(textView.TextDocument, marker.StartOffset, TextUtilities.LogicalDirection.Forward, TextUtilities.CaretPositioningMode.WordBorderOrSymbol);

                    if (endoffset == -1)
                    {
                        marker.Length = line.Length;
                    }
                    else
                    {
                        marker.EndOffset = endoffset;
                    }
                }

                foreach (Rect r in VisualLineGeometryBuilder.GetRectsForSegment(textView, marker))
                {
                    Point startPoint = r.BottomLeft;
                    Point endPoint = r.BottomRight;

                    var usedPen = new Pen(new SolidColorBrush(marker.MarkerColor), 1);
                    
                    const double offset = 2.5;

                    int count = Math.Max((int)((endPoint.X - startPoint.X) / offset) + 1, 4);

                    var geometry = new StreamGeometry();

                    using (StreamGeometryContext ctx = geometry.Open())
                    {
                        ctx.BeginFigure(startPoint, false);

                        foreach (var point in CreatePoints(startPoint, endPoint, offset, count))
                        {
                            ctx.LineTo(point);
                        }

                        ctx.EndFigure(false);
                    }

                    context.DrawGeometry(Brushes.Transparent, usedPen, geometry);
                    break;
                }
            }
        }

        private IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Point(start.X + (i * offset), start.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }

        public void Clear()
        {
            var toRemove = new List<TextMarker>();

            foreach (TextMarker marker in markers.ToList())
            {
                toRemove.Add(marker);
                markers.Remove(marker);
            }
        }

        public void Create(int offset, int length, string message, Color markerColor)
        {
            var m = new TextMarker(offset, length);
            markers.Add(m);
            m.MarkerColor = markerColor;
            m.ToolTip = message;            
        }

        public void Update()
        {
            if(this.DataChanged != null)
            {
                DataChanged(this, new EventArgs());
            }
        }

        public IEnumerable<TextMarker> GetMarkersAtOffset(int offset)
        {
            return markers == null ? Enumerable.Empty<TextMarker>() : markers.FindSegmentsContaining(offset);
        }
    }
}
