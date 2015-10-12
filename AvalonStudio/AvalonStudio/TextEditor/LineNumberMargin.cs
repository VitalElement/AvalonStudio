namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Media;
    using System;
    using System.Linq;

    class LineNumberMargin : Control
    {
        private TextView textView;

        public LineNumberMargin (TextView textView)
        {
            this.textView = textView;
        }

        public override void Render(DrawingContext context)
        {            
            var charPos = textView.FormattedText.HitTestTextPosition(0);

            var x = charPos.X;
            var y = charPos.Y ;
            var b = charPos.Bottom;
            var lineHeight = b - y;

            int visibleLines = (int)(Bounds.Height / lineHeight);

            Width = (Math.Ceiling(charPos.Right)) * visibleLines.ToString().Length;

            var firstLine = textView.GetLine(0) + 1;
            
            for (int i = 0; i < visibleLines; i++)
            {
                context.DrawText(Brush.Parse("#5d5d5d"), new Point(0, lineHeight * i), new FormattedText((i + firstLine).ToString(), "Consolas", textView.FontSize, FontStyle.Normal, TextAlignment.Right, FontWeight.Normal) { Constraint = new Size(Width, Bounds.Height) });
            }

            context.DrawLine(new Pen(Brush.Parse("#5d5d5d")), new Point(Width + 5, 0), new Point(Width + 5, Bounds.Height));
        }        
    }
}
