namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    using System;
    using Perspex;
    using Perspex.Media;
    using TextEditor.Rendering;

    class PragmaMarkTextLineTransformer : IDocumentLineTransformer
    {
        private SolidColorBrush pragmaBrush = new SolidColorBrush(Color.FromArgb(0x88, 0xB8, 0x48, 0xFF));
        private SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, VisualLine line)
        {
            if (line.RenderedText.Text.Contains("#pragma mark"))
            {
                int startIndex = line.RenderedText.Text.IndexOf("#pragma mark");

                line.RenderedText.SetForegroundBrush(pragmaBrush, startIndex, 12);
                line.RenderedText.SetForegroundBrush(brush, startIndex + 12, line.RenderedText.Text.Length - 12);
            }
            else if (line.RenderedText.Text.Contains("#pragma"))
            {
                int startIndex = line.RenderedText.Text.IndexOf("#pragma");

                line.RenderedText.SetForegroundBrush(pragmaBrush, startIndex, 7);
            }
        }
    }
}
