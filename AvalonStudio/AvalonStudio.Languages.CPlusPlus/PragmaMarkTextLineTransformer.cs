using Avalonia.Media;
using AvaloniaEdit.Rendering;
using System;
using System.Collections.Generic;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    internal class PragmaMarkTextLineTransformer : IVisualLineTransformer
    {
        private readonly SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));
        private readonly SolidColorBrush pragmaBrush = new SolidColorBrush(Color.FromArgb(0x88, 0xB8, 0x48, 0xFF));

#pragma warning disable 67

        public event EventHandler<EventArgs> DataChanged;

        public void Transform(ITextRunConstructionContext context, IList<VisualLineElement> elements)
        {
            throw new NotImplementedException();
        }

#pragma warning restore 67

        public void TransformLine(TextView textView, VisualLine line)
        {
            /*if (!line.RenderedText.Text.Trim().StartsWith("//"))
            {
                if (line.RenderedText.Text.Contains("#pragma mark"))
                {
                    var startIndex = line.RenderedText.Text.IndexOf("#pragma mark");

                    line.RenderedText.SetTextStyle(startIndex, 12, pragmaBrush);
                    line.RenderedText.SetTextStyle(startIndex + 12, line.RenderedText.Text.Length - 12, brush);
                }
                else if (line.RenderedText.Text.Contains("#pragma"))
                {
                    var startIndex = line.RenderedText.Text.IndexOf("#pragma");

                    line.RenderedText.SetTextStyle(startIndex, 7, pragmaBrush);
                }
            }*/
        }
    }
}