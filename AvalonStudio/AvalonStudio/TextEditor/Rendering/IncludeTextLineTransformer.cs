namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using Perspex.Media;
    using System;

    class IncludeTextLineTransformer : IDocumentLineTransformer
    {
        private Brush brush = Brush.Parse("#D69D85");

        public void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, DocumentLine line, FormattedText formattedText)
        {
            if (formattedText.Text.Contains("#include"))
            {
                int startIndex = formattedText.Text.IndexOf("#include");

                formattedText.SetForegroundBrush(brush, startIndex, formattedText.Text.Length - startIndex);                
            }
        }
    }
}
