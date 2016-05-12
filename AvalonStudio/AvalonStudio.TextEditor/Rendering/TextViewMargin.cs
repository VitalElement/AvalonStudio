namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Media;
    using Rendering;
    using System;

    public class TextInfo
    {
        public double LineHeight { get; set; }
        public double CharWidth { get; set; }
        public int NumLines { get; set; }
    }

    public abstract class TextViewMargin : Control
    {
        public TextViewMargin()
        {

        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            textView = Parent.Parent.Parent.Parent as TextView;

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
                TextInfo info = new TextInfo();

                var charRect = textView.CharSize;

                info.LineHeight = charRect.Height;
                info.CharWidth = charRect.Width;

                info.NumLines = textView.TextDocument.LineCount;

                Render(context, info);
            }
        }

        public abstract void Render(DrawingContext context, TextInfo textInfo);

        protected TextView textView;
    }
}
