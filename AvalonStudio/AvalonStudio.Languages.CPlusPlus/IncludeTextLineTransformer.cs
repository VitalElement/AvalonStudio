namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    using System;
    using Avalonia;
    using Avalonia.Media;
    using TextEditor.Rendering;
    class IncludeTextLineTransformer : IDocumentLineTransformer
    {
        private IBrush brush = Brush.Parse("#D69D85");

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, VisualLine line)
        {
            if (line.RenderedText.Text.Contains("#include"))
            {
                int startIndex = line.RenderedText.Text.IndexOf("#include");

                line.RenderedText.SetForegroundBrush(brush, startIndex, line.RenderedText.Text.Length - startIndex);                
            }
        }
    }
}
