namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex;
    using Perspex.Media;
    using System;

    class DefineTextLineTransformer : IDocumentLineTransformer
    {
        private SolidColorBrush pragmaBrush = new SolidColorBrush(Color.FromArgb(0x88, 0xB8, 0x48, 0xFF));
        private SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));

        public void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, DocumentLine line, FormattedText formattedText)
        {
            if (formattedText.Text.Contains("#define"))
            {
                int startIndex = formattedText.Text.IndexOf("#define");

                formattedText.SetForegroundBrush(pragmaBrush, startIndex, 7);
                formattedText.SetForegroundBrush(brush, startIndex + 7, formattedText.Text.Length - 7);
            }
        }
    }
}
