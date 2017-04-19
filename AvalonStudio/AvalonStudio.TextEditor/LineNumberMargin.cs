using Avalonia;
using Avalonia.Media;
using AvalonStudio.TextEditor.Document;
using System.Linq;

namespace AvalonStudio.TextEditor
{
    public class LineNumberMargin : TextViewMargin
    {
        private readonly IBrush foreground;
        private readonly IBrush currentLineForeground;

        public LineNumberMargin()
        {
            foreground = Brush.Parse("#2691AF");
            currentLineForeground = Brush.Parse("#2691AF");
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (double.IsNaN(Width))
            {
                Width = 0;
            }

            return new Size(Width, 0);
        }

        public override void Render(DrawingContext context, TextInfo textInfo)
        {
            if (textView.TextDocument != null)
            {
                Width = (textInfo.CharWidth * textInfo.NumLines.ToString().Length) + 12;

                if (textView != null && textView.VisualLines.Count > 0)
                {
                    var firstLine = textView.VisualLines.First().DocumentLine.LineNumber;
                    var lastLine = textView.VisualLines.Last().DocumentLine.LineNumber;

                    DocumentLine currentLine = null;

                    if (textView.SelectionStart == textView.SelectionEnd && textView.CaretIndex >= 0 && textView.CaretIndex <= textView.TextDocument.TextLength)
                    {
                        currentLine = textView.TextDocument.GetLineByOffset(textView.CaretIndex);
                    }

                    for (var i = 0; i < textInfo.NumLines && i + firstLine <= textView.TextDocument.LineCount && i + firstLine <= lastLine; i++)
                    {
                        using (
                            var formattedText = new FormattedText((i + firstLine).ToString(), "Consolas", textView.FontSize, FontStyle.Normal,
                                TextAlignment.Right, FontWeight.Normal)
                            { Constraint = new Size(Width, Bounds.Height) })
                        {
                            IBrush textColor = foreground;

                            if (currentLine != null)
                            {
                                if ((i + firstLine) == currentLine.LineNumber)
                                {
                                    textColor = currentLineForeground;
                                }
                            }

                            context.DrawText(textColor, new Point(-8, textInfo.LineHeight * i), formattedText);
                        }
                    }
                }
            }
        }
    }
}