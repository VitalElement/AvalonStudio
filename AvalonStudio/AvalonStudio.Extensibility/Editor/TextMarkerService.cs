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
                if (marker.Diagnostic.Level != DiagnosticLevel.Hidden)
                {
                    if (marker.EndOffset < textView.Document.TextLength)
                    {
                        foreach (var r in BackgroundGeometryBuilder.GetRectsForSegment(textView, marker))
                        {
                            if (marker.Diagnostic.Category == DiagnosticCategory.Style)
                            {
                                var usedPen = new Pen(new SolidColorBrush(marker.MarkerColor), 1);
                                drawingContext.DrawLine(usedPen, r.BottomLeft, r.BottomLeft.WithX(r.BottomLeft.X + 15));
                            }
                            else
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
                foreach (var m in markers.ToArray())
                {
                    if (predicate(m))
                        markers.Remove(m);
                }
            }
        }

        public void SetDiagnostics(object tag, TextSegmentCollection<Diagnostic> diagnostics)
        {            
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

                    case DiagnosticLevel.Info:
                        if(diag.Category == DiagnosticCategory.Style)
                        {
                            markerColor = Color.FromRgb(0xD4, 0xD4, 0xD4);
                        }
                        else
                        {
                            markerColor = Color.FromRgb(0, 25, 255);
                        }
                        break;

                    default:
                        markerColor = Color.FromRgb(0, 255, 74);
                        break;
                }                

                Create(diag, markerColor, tag);
            }
        }
        
        private void Create(Diagnostic diagnostic, Color markerColor, object tag)
        {
            var m = new TextMarker(diagnostic);

            markers.Add(m);
            
            m.MarkerColor = markerColor;
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

        public sealed class TextMarker : TextSegment
        {
            public TextMarker(Diagnostic diagnostic)
            {
                StartOffset = diagnostic.StartOffset;
                Length = diagnostic.Length;
                Diagnostic = diagnostic;
            }

            public Diagnostic Diagnostic { get; set; }            
            public Color? BackgroundColor { get; set; }
            public Color MarkerColor { get; set; }
            public string ToolTip { get; set; }
            public object Tag { get; set; }
        }
    }
}