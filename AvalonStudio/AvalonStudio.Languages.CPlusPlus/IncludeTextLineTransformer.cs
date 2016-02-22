namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    using System;
    using Perspex;
    using Perspex.Media;
    using TextEditor.Rendering;
    class IncludeTextLineTransformer : IDocumentLineTransformer
    {
        private Brush brush = Brush.Parse("#D69D85");

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, VisualLine line)
        {
            if (line.RenderedText.Text.Contains("#include"))
            {
                int startIndex = line.RenderedText.Text.IndexOf("#include");

                line.RenderedText.SetForegroundBrush(brush, startIndex, line.RenderedText.Text.Length - startIndex);                
            }
        }
    }
}
