namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using Perspex.Media;
    using System;

    class IncludeTextLineTransformer : IDocumentLineTransformer
    {
        private Brush brush = Brush.Parse("#D69D85");

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
