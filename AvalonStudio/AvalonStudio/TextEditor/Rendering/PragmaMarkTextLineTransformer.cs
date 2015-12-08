namespace AvalonStudio.TextEditor.Rendering
{
    using System;    
    using Document;
    using Perspex.Media;

    class PragmaMarkTextLineTransformer : IDocumentLineTransformer
    {
        private SolidColorBrush pragmaBrush = new SolidColorBrush(Color.FromArgb(0x88, 0xB8, 0x48, 0xFF));
        private SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));

        public void TransformLine(DocumentLine line, FormattedText formattedText)
        {
            if (formattedText.Text.Contains("#pragma mark"))
            {
                formattedText.SetForegroundBrush(pragmaBrush, 0, 12);
                formattedText.SetForegroundBrush(brush, 12, formattedText.Text.Length);                
            }
        }
    }
}
