using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;
using System;

namespace AvalonStudio.TextEditor
{
    public class TextInfo
    {
        public double LineHeight { get; set; }
        public double CharWidth { get; set; }
        public int NumLines { get; set; }
    }

    public abstract class TextViewMargin : Control
    {
        protected TextView textView;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            textView = Parent as TextView;

            if (textView == null)
            {
                throw new Exception("Margin must be contained inside a TextEditor control.");
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            textView = null;
        }

        public override void Render(DrawingContext context)
        {
            if (textView.TextDocument != null)
            {
                var info = new TextInfo();

                var charRect = textView.CharSize;

                info.LineHeight = charRect.Height;
                info.CharWidth = charRect.Width;

                info.NumLines = textView.TextDocument.LineCount;

                Render(context, info);
            }
        }

        public abstract void Render(DrawingContext context, TextInfo textInfo);
    }
}