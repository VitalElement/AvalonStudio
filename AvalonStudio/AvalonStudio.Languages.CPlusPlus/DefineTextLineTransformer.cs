namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    using System;
    using Avalonia;
    using Avalonia.Media;
    using TextEditor.Rendering;

    class DefineTextLineTransformer : IDocumentLineTransformer
    {
        private IBrush pragmaBrush = Brush.Parse("#68217A");
        private SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, VisualLine line)
        {
            if (line.RenderedText.Text.Contains("#define"))
            {
                int startIndex = line.RenderedText.Text.IndexOf("#define");

                line.RenderedText.SetForegroundBrush(pragmaBrush, startIndex, 7);
                line.RenderedText.SetForegroundBrush(brush, startIndex + 7, line.RenderedText.Text.Length - 7);
            }
        }
    }
}
