namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Input;
    using Perspex.Media;
    using System.Linq;

    public class BreakPointMargin : TextViewMargin
    {
        private bool previewPointVisible = false;

        static BreakPointMargin()
        {
            FocusableProperty.OverrideDefaultValue(typeof(BreakPointMargin), true);
        }

        public BreakPointMargin()
        {

        }

        public override void Render(DrawingContext context, TextInfo textInfo)
        {
            Width = textInfo.LineHeight;

            context.FillRectangle(Brush.Parse("#333333"), Bounds);

            if (previewPointVisible)
            {
                context.FillRectangle(Brush.Parse("#631912"), new Rect(Bounds.Size.Width / 4, textInfo.LineHeight * (BpLine - textView.VisualLines.First().DocumentLine.LineNumber) + Bounds.Size.Width / 4, Bounds.Size.Width / 1.5, textInfo.LineHeight / 1.5), (float)textInfo.LineHeight);
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            previewPointVisible = true;

            var offset = textView.GetOffsetFromPoint(e.GetPosition(this));

            if (offset != -1)
            {
                BpLine = textView.TextDocument.GetLineByOffset(offset).LineNumber; // convert from text line to visual line.
            }

            InvalidateVisual();

        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(100, availableSize.Height);
        }

        protected override void OnPointerLeave(PointerEventArgs e)
        {
            previewPointVisible = false;

            InvalidateVisual();
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            InvalidateVisual();
        }

        private int BpLine;
    }
}
