using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvaloniaEdit.Utils;
using AvalonStudio.Extensibility.Languages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Controls.Editor
{
    class ScopeLineBackgroundRenderer : IBackgroundRenderer
    {
        public KnownLayer Layer => KnownLayer.Background;

        private readonly IBrush brush = Brush.Parse("#717171");
        private readonly Pen _pen;

        private TextSegmentCollection<TextSegment> markers;

        private TextDocument _document;

        public ScopeLineBackgroundRenderer(TextDocument document)
        {
            _document = document;
            _pen = new Pen(brush, 1, new DashStyle(new double[] { 8, 4 }, 0));
            markers = new TextSegmentCollection<TextSegment>(document);
        }

        public void Dispose()
        {
            markers?.Clear();
            markers.Disconnect(_document);
            markers = null;
            _document = null;
        }

        public void ApplyIndex(IEnumerable<IndexEntry> index)
        {
            markers.Clear();

            if (index != null)
            {
                markers.AddRange(index.Select(s => new TextSegment { StartOffset = s.StartOffset, EndOffset = s.EndOffset }));
            }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            var typeface = textView.GetValue(TextBlock.FontFamilyProperty);
            var emSize = textView.GetValue(TextBlock.FontSizeProperty);

            var formattedText = TextFormatterFactory.CreateFormattedText(
                textView,
                "9",
                typeface,
                emSize,
                Brushes.Black
            );

            var charSize = formattedText.Bounds;
            var pixelSize = PixelSnapHelpers.GetPixelSize(textView);

            foreach (var entry in markers.Where(e => e.Length > 0))
            {
                if (entry.StartOffset <= textView.Document.TextLength)
                {
                    var startLine = textView.Document.GetLineByOffset(entry.StartOffset);

                    var start = entry.StartOffset;

                    var startChar = textView.Document.GetCharAt(startLine.Offset);

                    if (char.IsWhiteSpace(startChar))
                    {
                        start = TextUtilities.GetNextCaretPosition(textView.Document, startLine.Offset, LogicalDirection.Forward,
                                CaretPositioningMode.WordBorder);
                    }

                    var endLine = textView.Document.GetLineByOffset(entry.EndOffset <= textView.Document.TextLength ? entry.EndOffset : textView.Document.TextLength);

                    if (endLine.EndOffset > start && startLine != endLine)
                    {
                        var newEntry = new TextSegment() { StartOffset = start, EndOffset = endLine.EndOffset };

                        var rects = BackgroundGeometryBuilder.GetRectsForSegment(textView, newEntry);

                        var rect = GetRectForRange(rects);

                        if (!rect.IsEmpty)
                        {
                            var startColumn = textView.Document.GetLocation(newEntry.StartOffset).Column - 1;
                            var endColumn = textView.Document.GetLocation(newEntry.EndOffset).Column - 2;

                            var xPos = charSize.Width * Math.Min(startColumn, endColumn);

                            rect = rect.WithX(xPos + (charSize.Width / 2));

                            rect = rect.WithX(PixelSnapHelpers.PixelAlign(rect.X, pixelSize.Width));
                            rect = rect.WithY(PixelSnapHelpers.PixelAlign(rect.Y, pixelSize.Height));

                            drawingContext.DrawLine(_pen, rect.TopLeft, rect.BottomLeft);
                        }
                    }
                }
            }
        }

        public Rect GetRectForRange(IEnumerable<Rect> rects)
        {
            if (rects.Count() == 0)
            {
                return Rect.Empty;
            }

            var first = rects.FirstOrDefault();
            var last = rects.LastOrDefault();

            return first.WithHeight(last.Y - first.Y - first.Height).WithY(first.Y + first.Height);
        }
    }
}
