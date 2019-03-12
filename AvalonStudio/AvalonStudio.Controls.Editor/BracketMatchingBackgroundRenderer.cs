using Avalonia;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;

namespace AvalonStudio.Controls.Editor
{
    public class BracketMatchingBackgroundRenderer : IBackgroundRenderer
    {
        private readonly CodeEditor _editor;

        private readonly IBrush bracketHighlightBrush = Brush.Parse("#123e70");

        public BracketMatchingBackgroundRenderer(CodeEditor editor)
        {
            _editor = editor;
        }

        public KnownLayer Layer => KnownLayer.Background;

#pragma warning disable 67

        public event EventHandler<EventArgs> DataChanged;

        public int FindMatchingBracketForward(AvaloniaEdit.Document.TextDocument document, int startOffset, char open, char close)
        {
            var result = startOffset;

            var currentChar = document.GetCharAt(startOffset++);

            if (currentChar == open)
            {
                var numOpen = 0;

                while (true)
                {
                    if (startOffset >= document.TextLength)
                    {
                        break;
                    }

                    currentChar = document.GetCharAt(startOffset++);

                    if (currentChar == close && numOpen == 0)
                    {
                        result = startOffset - 1;
                        break;
                    }
                    if (currentChar == open)
                    {
                        numOpen++;
                    }
                    else if (currentChar == close)
                    {
                        numOpen--;
                    }

                    if (startOffset >= document.TextLength)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public int FindMatchingBracketBackward(AvaloniaEdit.Document.TextDocument document, int startOffset, char close, char open)
        {
            var result = startOffset;

            var currentChar = document.GetCharAt(startOffset--);

            if (currentChar == close)
            {
                var numOpen = 0;

                while (true)
                {
                    if (startOffset < 0)
                    {
                        break;
                    }

                    currentChar = document.GetCharAt(startOffset--);

                    if (currentChar == open && numOpen == 0)
                    {
                        result = startOffset + 1;
                        break;
                    }
                    if (currentChar == close)
                    {
                        numOpen++;
                    }
                    else if (currentChar == open)
                    {
                        numOpen--;
                    }

                    if (startOffset >= document.TextLength)
                    {
                        break;
                    }
                }
            }

            return result;
        }


#pragma warning restore 67

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            var offset = _editor.CaretOffset;
            var document = _editor.Document;
                        
            if (offset != -1 && document != null)
            {
                var caretChar = '\0';
                var behindCaretChar = '\0';

                if (offset < document.TextLength)
                {
                    caretChar = document.GetCharAt(offset);
                }

                if (offset - 1 > 0 && offset < document.TextLength)
                {
                    behindCaretChar = document.GetCharAt(offset - 1);
                }

                if (caretChar.IsOpenBracketChar() && !caretChar.IsPunctuationChar())
                {
                    var closeOffset = FindMatchingBracketForward(document, offset, caretChar,
                        caretChar.GetCloseBracketChar());

                    Highlight(drawingContext,
                        BackgroundGeometryBuilder.GetRectsForSegment(textView,
                            new TextSegment { StartOffset = offset, EndOffset = offset + 1 }));
                    Highlight(drawingContext,
                        BackgroundGeometryBuilder.GetRectsForSegment(textView,
                            new TextSegment { StartOffset = closeOffset, EndOffset = closeOffset + 1 }));
                }

                if (behindCaretChar.IsCloseBracketChar() && !behindCaretChar.IsPunctuationChar())
                {
                    var openOffset = FindMatchingBracketBackward(document, offset - 1, behindCaretChar,
                        behindCaretChar.GetOpenBracketChar());

                    Highlight(drawingContext,
                        BackgroundGeometryBuilder.GetRectsForSegment(textView,
                            new TextSegment { StartOffset = offset - 1, EndOffset = offset }));
                    Highlight(drawingContext,
                        BackgroundGeometryBuilder.GetRectsForSegment(textView,
                            new TextSegment { StartOffset = openOffset, EndOffset = openOffset + 1 }));
                }
            }
        }

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
        }

        private void Highlight(DrawingContext drawingContext, IEnumerable<Rect> rects)
        {
            if (ColorScheme.CurrentColorScheme != null)
            {
                foreach (var rect in rects)
                {
                    drawingContext.FillRectangle(ColorScheme.CurrentColorScheme.BracketMatch, rect);
                }
            }
        }
    }
}