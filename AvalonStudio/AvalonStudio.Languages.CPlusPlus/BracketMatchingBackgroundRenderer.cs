namespace AvalonStudio.Languages.CPlusPlus
{
    using AvalonStudio.TextEditor.Rendering;
    using Perspex;
    using Perspex.Media;
    using System.Collections.Generic;
    using TextEditor.Document;
    using Utils;
    using System;

    public class BracketMatchingBackgroundRenderer : IBackgroundRenderer
    {
        private IBrush bracketHighlightBrush = Brush.Parse("#123e70");

        public event EventHandler<EventArgs> DataChanged;

        private void Highlight(DrawingContext drawingContext, IEnumerable<Rect> rects)
        {
            foreach (var rect in rects)
            {
                drawingContext.FillRectangle(bracketHighlightBrush, rect);
            }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
           
            if (textView.CaretIndex != -1)
            {
                char caretChar = '\0';
                char behindCaretChar = '\0';

                if (textView.CaretIndex < textView.TextDocument.TextLength)
                {
                    caretChar = textView.TextDocument.GetCharAt(textView.CaretIndex);
                }
                
                if (textView.CaretIndex - 1 > 0)
                {
                    behindCaretChar = textView.TextDocument.GetCharAt(textView.CaretIndex - 1);
                }

                if (caretChar.IsOpenBracketChar() && !caretChar.IsPunctuationChar())
                {
                    int closeOffset = textView.FindMatchingBracketForward(textView.CaretIndex, caretChar, caretChar.GetCloseBracketChar());

                    Highlight(drawingContext, VisualLineGeometryBuilder.GetRectsForSegment(textView, new TextSegment() { StartOffset = textView.CaretIndex, EndOffset = textView.CaretIndex + 1 }));
                    Highlight(drawingContext, VisualLineGeometryBuilder.GetRectsForSegment(textView, new TextSegment() { StartOffset = closeOffset, EndOffset = closeOffset + 1 }));
                }

                if (behindCaretChar.IsCloseBracketChar() && !behindCaretChar.IsPunctuationChar())
                {
                    int openOffset = textView.FindMatchingBracketBackward(textView.CaretIndex - 1, behindCaretChar, behindCaretChar.GetOpenBracketChar());

                    Highlight(drawingContext, VisualLineGeometryBuilder.GetRectsForSegment(textView, new TextSegment() { StartOffset = textView.CaretIndex - 1, EndOffset = textView.CaretIndex }));
                    Highlight(drawingContext, VisualLineGeometryBuilder.GetRectsForSegment(textView, new TextSegment() { StartOffset = openOffset, EndOffset = openOffset + 1 }));
                }
            }
        }

        public void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line)
        {
            
        }
    }
}
