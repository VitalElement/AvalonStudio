namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Media;
    using System.Linq;

    public class LineNumberMargin : TextViewMargin
    {
        public LineNumberMargin()
        {

        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (double.IsNaN(Width))
            {
                Width = 0;
            }

            return new Size(Width, availableSize.Height);
        }

        public override void Render(DrawingContext context, TextInfo textInfo)
        {
            if (textView.TextDocument != null)
            {                
                Width = (textInfo.CharWidth * textInfo.NumLines.ToString().Length) + 8;

                if (textView != null && textView.VisualLines.Count > 0)
                {
                    var firstLine = textView.VisualLines.First().DocumentLine.LineNumber;

                    for (int i = 0; i < textInfo.NumLines && (i + firstLine) <= textView.TextDocument.LineCount; i++)
                    {
                        context.DrawText(Brush.Parse("#5d5d5d"), new Point(-4, textInfo.LineHeight * i), new FormattedText((i + firstLine).ToString(), "Consolas", textView.FontSize, FontStyle.Normal, TextAlignment.Right, FontWeight.Normal) { Constraint = new Size(Width, Bounds.Height) });
                    }

                    context.DrawLine(new Pen(Brush.Parse("#5d5d5d")), new Point(Width, 0), new Point(Width, Bounds.Height));
                }
            }
        }
    }
}
