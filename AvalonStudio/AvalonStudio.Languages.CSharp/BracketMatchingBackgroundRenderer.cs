using Avalonia;
using Avalonia.Media;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Rendering;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;

namespace AvalonStudio.Languages.CPlusPlus
{
    public class BracketMatchingBackgroundRenderer : IBackgroundRenderer
    {
        private readonly IBrush bracketHighlightBrush = Brush.Parse("#123e70");

#pragma warning disable 67

        public event EventHandler<EventArgs> DataChanged;

#pragma warning restore 67

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (textView.CaretIndex != -1)
            {
                var caretChar = '\0';
                var behindCaretChar = '\0';

                if (textView.CaretIndex < textView.TextDocument.TextLength)
                {
                    caretChar = textView.TextDocument.GetCharAt(textView.CaretIndex);
                }

                if (textView.CaretIndex - 1 > 0 && textView.CaretIndex < textView.TextDocument.TextLength)
                {
                    behindCaretChar = textView.TextDocument.GetCharAt(textView.CaretIndex - 1);
                }

                if (caretChar.IsOpenBracketChar() && !caretChar.IsPunctuationChar())
                {
                    var closeOffset = textView.FindMatchingBracketForward(textView.CaretIndex, caretChar,
                        caretChar.GetCloseBracketChar());

                    Highlight(drawingContext,
                        VisualLineGeometryBuilder.GetRectsForSegment(textView,
                            new TextSegment { StartOffset = textView.CaretIndex, EndOffset = textView.CaretIndex + 1 }));
                    Highlight(drawingContext,
                        VisualLineGeometryBuilder.GetRectsForSegment(textView,
                            new TextSegment { StartOffset = closeOffset, EndOffset = closeOffset + 1 }));
                }

                if (behindCaretChar.IsCloseBracketChar() && !behindCaretChar.IsPunctuationChar())
                {
                    var openOffset = textView.FindMatchingBracketBackward(textView.CaretIndex - 1, behindCaretChar,
                        behindCaretChar.GetOpenBracketChar());

                    Highlight(drawingContext,
                        VisualLineGeometryBuilder.GetRectsForSegment(textView,
                            new TextSegment { StartOffset = textView.CaretIndex - 1, EndOffset = textView.CaretIndex }));
                    Highlight(drawingContext,
                        VisualLineGeometryBuilder.GetRectsForSegment(textView,
                            new TextSegment { StartOffset = openOffset, EndOffset = openOffset + 1 }));
                }
            }
        }

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
        }

        private void Highlight(DrawingContext drawingContext, IEnumerable<Rect> rects)
        {
            foreach (var rect in rects)
            {
                drawingContext.FillRectangle(bracketHighlightBrush, rect);
            }
        }
    }
}