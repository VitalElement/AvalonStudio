namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Input;
    using Perspex.Media;
    using Rendering;
    class BreakPointMargin : TextEditorMargin
    {
        private bool previewPointVisible = false;

        static BreakPointMargin()
        {
            FocusableProperty.OverrideDefaultValue(typeof(BreakPointMargin), true);
        }

        public BreakPointMargin ()
        {
            
        }

        public override void Render(DrawingContext context, TextInfo textInfo)
        {            
            Width = textInfo.LineHeight;

            context.FillRectangle(Brush.Parse("#333333"), Bounds);

            if (previewPointVisible)
            {                
                context.FillRectangle(Brush.Parse("#631912"), new Rect(Bounds.Size.Width / 4, textInfo.LineHeight * BpLine + Bounds.Size.Width / 4, Bounds.Size.Width / 1.5, textInfo.LineHeight / 1.5), (float)textInfo.LineHeight);
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            previewPointVisible = true;

            var offset = textEditor.TextView.GetOffsetFromPoint(e.GetPosition(this));

            if (offset != -1)
            {
                BpLine = textEditor.TextDocument.GetLineByOffset(offset).LineNumber - 1; // convert from text line to visual line.
            }

            InvalidateVisual();
            
        }

        protected override void OnPointerLeave(PointerEventArgs e)
        {
            previewPointVisible = false;

            InvalidateVisual();
        }

        protected override void OnPointerPressed(PointerPressEventArgs e)
        {
            //var result = textEditor.TextView.FormattedText.HitTestPoint(e.GetPosition(this));
           // var line = textEditor.TextView.GetLine(result.TextPosition);

            InvalidateVisual();
        }

        private int BpLine;
    }
}
