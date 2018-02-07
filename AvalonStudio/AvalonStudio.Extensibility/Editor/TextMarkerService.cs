using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Languages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Extensibility.Editor
{
    public class TextMarkerService : IBackgroundRenderer
    {
        private readonly TextSegmentCollection<TextMarker> markers;

        public KnownLayer Layer => KnownLayer.Background;

        public TextMarkerService(TextDocument document)
        {
            markers = new TextSegmentCollection<TextMarker>(document);
        }

        public event EventHandler<EventArgs> DataChanged;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (markers == null)
            {
                return;
            }

            var visualLines = textView.VisualLines;
            if (visualLines.Count == 0)
            {
                return;
            }

            int viewStart = visualLines.First().FirstDocumentLine.Offset;
            int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

            int start = Math.Min(viewStart, viewEnd);
            int end = Math.Max(viewStart, viewEnd);

            foreach (TextMarker marker in markers.FindOverlappingSegments(start, end - start))
            {
                if (marker.EndOffset < textView.Document.TextLength)
                {
                    foreach (var r in BackgroundGeometryBuilder.GetRectsForSegment(textView, marker))
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

        public void SetDiagnostics(TextSegmentCollection<Diagnostic> diagnostics)
        {
            Clear();

            foreach (var diag in diagnostics)
            {
                Color markerColor;

                switch (diag.Level)
                {
                    case DiagnosticLevel.Error:
                    case DiagnosticLevel.Fatal:
                        markerColor = Color.FromRgb(253, 45, 45);
                        break;

                    case DiagnosticLevel.Warning:
                        markerColor = Color.FromRgb(255, 207, 40);
                        break;

                    default:
                        markerColor = Color.FromRgb(0, 42, 74);
                        break;
                }

                Create(diag.StartOffset, diag.Length, diag.Spelling, markerColor);
            }
        }
        
        private void Create(int offset, int length, string message, Color markerColor)
        {
            var m = new TextMarker(offset, length);
            markers.Add(m);
            m.MarkerColor = markerColor;
            m.ToolTip = message;
        }

        public void Update()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
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