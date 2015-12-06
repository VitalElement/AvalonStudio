using AvalonStudio.TextEditor.Document;
using Perspex;
using Perspex.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.TextEditor.Rendering
{
    public class TextMarkerService : IBackgroundRenderer
    {
        private readonly TextView textView;
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

        public TextMarkerService(TextView textView)
        {
            this.textView = textView;
            
        }

        public void Install(TextDocument doc)
        {
            markers = new TextSegmentCollection<TextMarker>(doc);
            Create(591, 7, "This is an error", Color.FromRgb(243, 45, 45));
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if(markers == null)
            {
                return;
            }

            int viewStart = textView.TextDocument.Lines.First().Offset;
            int viewEnd = textView.TextDocument.Lines.Last().EndOffset;

            foreach (TextMarker marker in markers.FindOverlappingSegments(viewStart, viewEnd - viewStart))
            {
                //if (marker.BackgroundColor != null)
                //{
                //    var geoBuilder = new BackgroundGeometryBuilder { AlignToWholePixels = true, CornerRadius = 3 };
                //    geoBuilder.AddSegment(textView, marker);
                //    Geometry geometry = geoBuilder.CreateGeometry();
                //    if (geometry != null)
                //    {
                //        Color color = marker.BackgroundColor.Value;
                //        var brush = new SolidColorBrush(color);
                //        brush.Freeze();
                //        drawingContext.DrawGeometry(brush, null, geometry);
                //    }
                //}

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

            foreach (TextMarker marker in toRemove)
            {
                //Redraw(marker);
            }
        }

        public void Create(int offset, int length, string message, Color markerColor)
        {
            var m = new TextMarker(offset, length);
            markers.Add(m);
            m.MarkerColor = markerColor;
            m.ToolTip = message;
            //Redraw(m);
        }

        public IEnumerable<TextMarker> GetMarkersAtOffset(int offset)
        {
            return markers == null ? Enumerable.Empty<TextMarker>() : markers.FindSegmentsContaining(offset);
        }
    }
}
