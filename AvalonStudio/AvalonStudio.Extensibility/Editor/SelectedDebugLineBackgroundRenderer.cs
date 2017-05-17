using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using System;

namespace AvalonStudio.TextEditor.Rendering
{
    public class SelectedDebugLineBackgroundRenderer : IBackgroundRenderer
    {
        private int _line;
        private readonly IBrush selectedLineBg;
        private int _startColumn;
        private int _endColumn;

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
        }

        public int Line
        {
            get
            {
                return _line;
            }
            set
            {
                _line = value;

                DataChanged?.Invoke(this, new EventArgs());
            }
        }

        public int StartColumn
        {
            get
            {
                return _startColumn;
            }
            set
            {
                _startColumn = value;
                DataChanged?.Invoke(this, new EventArgs());
            }
        }

        public int EndColumn
        {
            get
            {
                return _endColumn;
            }
            set
            {
                _endColumn = value;
                DataChanged?.Invoke(this, new EventArgs());
            }
        }

        public KnownLayer Layer => KnownLayer.Background;

        public event EventHandler<EventArgs> DataChanged;

        public void Draw(TextView textView, DrawingContext drawingContext)
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

               /* var rects = VisualLineGeometryBuilder.GetRectsForSegment(textView, segment);

                foreach (var rect in rects)
                {
                    var drawRect = new Rect(rect.TopLeft.X - 1, rect.TopLeft.Y - 1, rect.Width + 2, rect.Height + 2);
                    drawingContext.FillRectangle(selectedLineBg, drawRect);
                }*/
            }
        }

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
           /* if (line.DocumentLine.LineNumber == Line)
            {
                line.RenderedText.SetTextStyle(StartColumn - 1, EndColumn - StartColumn, Brushes.Black);
            }*/
        }
    }
}