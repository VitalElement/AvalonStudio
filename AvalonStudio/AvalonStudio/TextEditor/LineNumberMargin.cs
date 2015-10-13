namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Media;
    using System;
    using System.Linq;

    class LineNumberMargin : TextEditorMargin
    {
        public LineNumberMargin()
        {
            
        }

        public override void Render(DrawingContext context, TextInfo textInfo)
        {            
            var charPos = textEditor.TextView.FormattedText.HitTestTextPosition(0);

            Width = textInfo.CharWidth * textInfo.NumLines.ToString().Length + 5;

            var firstLine = textEditor.TextView.GetLine(0) + 1;
            
            for (int i = 0; i < textInfo.NumLines; i++)
            {
                context.DrawText(Brush.Parse("#5d5d5d"), new Point(-5, textInfo.LineHeight * i), new FormattedText((i + firstLine).ToString(), "Consolas", textEditor.FontSize, FontStyle.Normal, TextAlignment.Right, FontWeight.Normal) { Constraint = new Size(Width, Bounds.Height) });
            }

            context.DrawLine(new Pen(Brush.Parse("#5d5d5d")), new Point(Width, 0), new Point(Width, Bounds.Height));
            
        }


    }
}
