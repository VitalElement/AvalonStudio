using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;
using System;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    internal class DefineTextLineTransformer : IDocumentLineTransformer
    {
        private readonly SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));
        private readonly IBrush pragmaBrush = Brush.Parse("#9B9B9B");

#pragma warning disable 67

        public event EventHandler<EventArgs> DataChanged;

#pragma warning restore 67

        public void TransformLine(TextView textView, VisualLine line)
        {
            var trimmed = line.RenderedText.Text.Trim();

            if (trimmed.StartsWith("#") && !trimmed.StartsWith("#include"))
            {
                var startIndex = line.RenderedText.Text.IndexOf("#");

                var firstEndOffset = line.RenderedText.Text.IndexOf(" ", startIndex);

               // line.RenderedText.SetForegroundBrush(pragmaBrush, startIndex, firstEndOffset - startIndex);

                //var lastWordOffset = firstEndOffset != -1 ? line.RenderedText.Text.LastIndexOf(" ", firstEndOffset) + 1 : -1;

                //if (lastWordOffset != -1)
                //{
                //    line.RenderedText.SetForegroundBrush(brush, lastWordOffset, line.RenderedText.Text.Length - lastWordOffset);
                //}
            }
        }
    }
}