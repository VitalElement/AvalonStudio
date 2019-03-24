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
        private TextSegmentCollection<TextMarker> markers;

        public KnownLayer Layer => KnownLayer.Background;

        public ColorScheme ColorScheme { get; set; }

        private TextDocument _document;

        public TextMarkerService(TextDocument document)
        {
            _document = document;
            markers = new TextSegmentCollection<TextMarker>(document);

            ColorScheme = ColorScheme.Default;
        }

        public void Dispose()
        {
            markers.Clear();
            markers.Disconnect(_document);
            markers = null;
            _document = null;
        }

        public event EventHandler<EventArgs> DataChanged;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (markers == null || markers.Count == 0)
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
                if (marker.Diagnostic.Level != DiagnosticLevel.Hidden && marker.Length >= 0)
                {
                    if (marker.EndOffset < textView.Document.TextLength)
                    {
                        foreach (var r in BackgroundGeometryBuilder.GetRectsForSegment(textView, marker))
                        {
                            if (marker.Diagnostic.Category == DiagnosticCategory.Style)
                            {
                                var usedPen = new Pen(marker.Brush, 1);
                                drawingContext.DrawLine(usedPen, r.BottomLeft, r.BottomLeft.WithX(r.BottomLeft.X + 15));
                            }
                            else
                            {
                                var startPoint = r.BottomLeft;
                                var endPoint = r.BottomRight;

                                var usedPen = new Pen(marker.Brush, 1);

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
            }
        }

        private IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
        {
            for (var i = 0; i < count; i++)
            {
                yield return new Point(start.X + (i * offset), start.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }

        public void RemoveAll(Predicate<TextMarker> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (markers != null)
            {
                var toRemove = markers.Where(t => predicate(t)).ToArray();

                foreach (var m in toRemove)
                {
                    markers.Remove(m);
                }
            }
        }

        public void SetDiagnostics(object tag, IEnumerable<Diagnostic> diagnostics)
        {
            foreach (var diag in diagnostics)
            {
                IBrush markerColor;

                switch (diag.Level)
                {
                    case DiagnosticLevel.Error:
                    case DiagnosticLevel.Fatal:
                        markerColor = ColorScheme.ErrorDiagnostic;
                        break;

                    case DiagnosticLevel.Warning:
                        markerColor = ColorScheme.WarningDiagnostic;
                        break;

                    case DiagnosticLevel.Info:
                        if (diag.Category == DiagnosticCategory.Style)
                        {
                            markerColor = ColorScheme.StyleDiagnostic;
                        }
                        else
                        {
                            markerColor = ColorScheme.InfoDiagnostic;
                        }
                        break;

                    default:
                        markerColor = Brushes.Green;
                        break;
                }

                Create(diag, markerColor, tag);
            }
        }

        private void Create(Diagnostic diagnostic, IBrush markerColor, object tag)
        {
            var m = new TextMarker(diagnostic);

            markers.Add(m);

            m.Brush = markerColor;
            m.ToolTip = diagnostic.Spelling;
            m.Tag = tag;
        }

        public void Update()
        {
            DataChanged?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerable<TextMarker> GetMarkersAtOffset(int offset)
        {
            return markers == null ? Enumerable.Empty<TextMarker>() : markers.FindSegmentsContaining(offset);
        }

        public IEnumerable<TextMarker> FindOverlappingMarkers(ISegment segment)
        {
            return markers == null ? Enumerable.Empty<TextMarker>() : markers.FindOverlappingSegments(segment);
        }

        public sealed class TextMarker : TextSegment
        {
            public TextMarker(Diagnostic diagnostic)
            {
                StartOffset = diagnostic.StartOffset;
                Length = diagnostic.Length;
                Diagnostic = diagnostic;
            }

            public Diagnostic Diagnostic { get; set; }
            public IBrush Brush { get; set; }
            public string ToolTip { get; set; }
            public object Tag { get; set; }
        }
    }
}