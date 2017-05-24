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
        private readonly Dictionary<Diagnostic, TextMarker> _markerLinks;
        private readonly TextDocument _document;

        public KnownLayer Layer => KnownLayer.Caret;

        public TextMarkerService(TextDocument document)
        {
            _document = document;
            markers = new TextSegmentCollection<TextMarker>(document);
            _markerLinks = new Dictionary<Diagnostic, TextMarker>();
        }

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

            foreach (TextMarker marker in markers.FindOverlappingSegments(viewStart, viewEnd - viewStart))
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

        public void Create(Diagnostic diagnostic)
        {
            Color markerColor;

            switch (diagnostic.Level)
            {
                case DiagnosticLevel.Error:
                case DiagnosticLevel.Fatal:
                    markerColor = Color.FromRgb(253, 45, 45);
                    break;

                case DiagnosticLevel.Warning:
                    markerColor = Color.FromRgb(255, 207, 40);
                    break;

                default:
                    markerColor = Color.FromRgb(27, 161, 226);
                    break;
            }

            if(diagnostic.StartOffset == 0)
            {
                diagnostic.StartOffset = _document.GetOffset(diagnostic.Line, diagnostic.Column);
            }

            if(diagnostic.Length == 0)
            {
                diagnostic.Length = _document.GetLineByNumber(diagnostic.Line).Length;
            }

            var m = new TextMarker(diagnostic.StartOffset, diagnostic.Length);
            markers.Add(m);
            m.MarkerColor = markerColor;
            m.ToolTip = diagnostic.Spelling;

            _markerLinks.Add(diagnostic, m);
        }

        public void Remove(Diagnostic diagnostic)
        {
            markers.Remove(_markerLinks[diagnostic]);
            _markerLinks.Remove(diagnostic);
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