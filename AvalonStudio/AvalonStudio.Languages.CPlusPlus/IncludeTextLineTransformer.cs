using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;
using System;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    internal class IncludeTextLineTransformer : IDocumentLineTransformer
    {
        private readonly IBrush brush = Brush.Parse("#D69D85");
        private readonly IBrush pragmaBrush = Brush.Parse("#9B9B9B");

#pragma warning disable 67

        public event EventHandler<EventArgs> DataChanged;

#pragma warning restore 67

        public void TransformLine(TextView textView, VisualLine line)
        {
            if (line.RenderedText.Text.Contains("#include") && !line.RenderedText.Text.Trim().StartsWith("//"))
            {
                var startIndex = line.RenderedText.Text.IndexOf("#include");
                
                line.RenderedText.SetTextStyle(startIndex, 8, pragmaBrush);
                line.RenderedText.SetTextStyle(startIndex + 9, line.RenderedText.Text.Length - startIndex, brush);
            }
        }
    }
}