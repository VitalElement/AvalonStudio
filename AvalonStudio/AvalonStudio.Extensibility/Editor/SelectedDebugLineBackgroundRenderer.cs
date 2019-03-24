using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.CodeEditor;
using System;
using System.Collections.Generic;

namespace AvalonStudio.TextEditor.Rendering
{
    public class SelectedDebugLineBackgroundRenderer : GenericLineTransformer, IBackgroundRenderer
    {
        private int _line;
        private readonly IBrush selectedLineBg;
        private int _startColumn;
        private int _endColumn;
        private TextView _owner;

        public SelectedDebugLineBackgroundRenderer()
        {
            selectedLineBg = Brush.Parse("#C5C870");
            _startColumn = -1;
            _endColumn = -1;
        }

        public void SetLocation(int line, int startColumn = -1, int endColumn = -1)
        {
            _line = line;
            _startColumn = startColumn;
            _endColumn = endColumn;
            _owner?.Redraw();
        }

        public int Line
        {
            get
            {
                return _line;
            }
        }

        public int StartColumn
        {
            get
            {
                return _startColumn;
            }
        }

        public int EndColumn
        {
            get
            {
                return _endColumn;
            }
        }

        public KnownLayer Layer => KnownLayer.Background;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            _owner = textView;

            if (textView.VisualLinesValid && textView.Document != null)
            {
                if (_line > 0 && _line < textView.Document.LineCount)
                {
                    var currentLine = textView.Document.GetLineByNumber(_line);

                    var segment = new TextSegment();
                    segment.StartOffset = currentLine.Offset;
                    segment.EndOffset = currentLine.EndOffset;

                    if (_startColumn != -1 && _endColumn != -1)
                    {
                        segment.StartOffset = textView.Document.GetOffset(_line, _startColumn);
                        segment.EndOffset = textView.Document.GetOffset(_line, _endColumn);
                    }
                    else
                    {
                        _startColumn = textView.Document.GetLocation(segment.StartOffset).Column;
                        _endColumn = textView.Document.GetLocation(segment.EndOffset).Column;
                    }

                    var rects = BackgroundGeometryBuilder.GetRectsForSegment(textView, segment);

                    foreach (var rect in rects)
                    {
                        var drawRect = new Rect(rect.TopLeft.X - 1, rect.TopLeft.Y - 1, rect.Width + 2, rect.Height + 2);
                        drawingContext.FillRectangle(selectedLineBg, drawRect);
                    }
                }
            }
        }

        protected override void TransformLine(DocumentLine line, ITextRunConstructionContext context)
        {
            if (line.LineNumber == Line)
            {
                SetTextStyle(line, StartColumn - 1, EndColumn - StartColumn, Brushes.Black);
            }
        }
    }
}