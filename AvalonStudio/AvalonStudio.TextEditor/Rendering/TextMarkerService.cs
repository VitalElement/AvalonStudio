using Avalonia;
using Avalonia.Media;
using AvalonStudio.TextEditor.Document;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.TextEditor.Rendering
{
    public class TextMarkerService : IBackgroundRenderer
    {
        private readonly TextSegmentCollection<TextMarker> markers;

        public TextMarkerService(TextDocument document)
        {
            markers = new TextSegmentCollection<TextMarker>(document);
        }

        public event EventHandler<EventArgs> DataChanged;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
        }

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
            if (markers == null)
            {
                return;
            }

            var markersInLine = markers.FindOverlappingSegments(line);

            foreach (var marker in markersInLine)
            {
                if (marker.EndOffset < textView.TextDocument.TextLength)
                {
                    foreach (var r in VisualLineGeometryBuilder.GetRectsForSegment(textView, marker))
                    {
                        var startPoint = r.BottomLeft;
                        var endPoint = r.BottomRight;

                        var usedPen = new Pen(new SolidColorBrush(marker.MarkerColor), 1);

                        const double offset = 2.5;

                        var count = Math.Max((int)((endPoint.X - startPoint.X) / offset) + 1, 4);

                        var geometry = new StreamGeometry();

                        using (var ctx = geometry.Open())
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
        }

        private IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return new Point(start.X + (i * offset), start.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }

        public void Clear()
        {
            var toRemove = new List<TextMarker>();

            foreach (var marker in markers.ToList())
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
            if (DataChanged != null)
            {
                DataChanged(this, new EventArgs());
            }
        }

        public IEnumerable<TextMarker> GetMarkersAtOffset(int offset)
        {
            return markers == null ? Enumerable.Empty<TextMarker>() : markers.FindSegmentsContaining(offset);
        }

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
    }
}