namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex;
    using Perspex.Media;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TextMarkerService : IBackgroundRenderer
    {
        private readonly TextEditor editor;

        private TextSegmentCollection<TextMarker> markers;

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

        public TextMarkerService(TextEditor textView)
        {
            this.editor = textView;
            markers = new TextSegmentCollection<TextMarker>(textView.TextDocument);

        }

        public void UpdateOffsets(DocumentChangeEventArgs e)
        {
            if (markers != null)
            {
                markers.UpdateOffsets(e);
            }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if(markers == null)
            {
                return;
            }

            int viewStart = textView.VisualLines.First().Offset;
            int viewEnd = textView.VisualLines.Last().Offset;

            var markersOnScreen = markers.FindOverlappingSegments(viewStart, viewEnd - viewStart);

            foreach (TextMarker marker in markersOnScreen)
            {
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

                    drawingContext.DrawGeometry(Brushes.Transparent, usedPen, geometry);
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

        public IEnumerable<TextMarker> GetMarkersAtOffset(int offset)
        {
            return markers == null ? Enumerable.Empty<TextMarker>() : markers.FindSegmentsContaining(offset);
        }
    }
}
