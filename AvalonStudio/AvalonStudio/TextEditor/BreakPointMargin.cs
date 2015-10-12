namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Input;
    using Perspex.Media;

    class BreakPointMargin : TextEditorMargin
    {
        private bool previewPointVisible = false;

        static BreakPointMargin()
        {
            FocusableProperty.OverrideDefaultValue(typeof(BreakPointMargin), true);
        }

        public BreakPointMargin (TextView textView) : base (textView)
        {
            
        }

        public override void Render(DrawingContext context, TextInfo textInfo)
        {            
            Width = textInfo.LineHeight;

            if (previewPointVisible)
            {
                context.FillRectangle(Brush.Parse("#631912"), new Rect(Bounds.Size.Width / 4, textInfo.LineHeight * BpLine + Bounds.Size.Width / 4, Bounds.Size.Width / 1.5, textInfo.LineHeight / 1.5), (float)textInfo.LineHeight);
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            previewPointVisible = true;
            var result = textView.FormattedText.HitTestPoint(e.GetPosition(this));
            var line = textView.GetLine(result.TextPosition);

            BpLine = line;

            InvalidateVisual();
            
        }

        protected override void OnPointerLeave(PointerEventArgs e)
        {
            previewPointVisible = false;

            InvalidateVisual();
        }

        protected override void OnPointerPressed(PointerPressEventArgs e)
        {
            var result = textView.FormattedText.HitTestPoint(e.GetPosition(this));
            var line = textView.GetLine(result.TextPosition);

            InvalidateVisual();
        }

        private int BpLine;
    }
}
