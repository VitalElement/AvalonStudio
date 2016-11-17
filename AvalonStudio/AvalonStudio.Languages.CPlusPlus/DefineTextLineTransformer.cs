using System;
using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    internal class DefineTextLineTransformer : IDocumentLineTransformer
    {
        private readonly SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));
        private readonly IBrush pragmaBrush = Brush.Parse("#68217A");

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, VisualLine line)
        {
            var trimmed = line.RenderedText.Text.Trim();

            if (trimmed.StartsWith("#") && !trimmed.StartsWith("#include"))
            {
                var startIndex = line.RenderedText.Text.IndexOf("#");

                var firstEndOffset = line.RenderedText.Text.IndexOf(" ", startIndex);

                var lastWordOffset = firstEndOffset != -1 ? line.RenderedText.Text.LastIndexOf(" ", firstEndOffset) + 1 : -1;

                if (lastWordOffset != -1)
                {
                    line.RenderedText.SetForegroundBrush(brush, lastWordOffset, line.RenderedText.Text.Length - lastWordOffset);
                }
            }
        }
    }
}