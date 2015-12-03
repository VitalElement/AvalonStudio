namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex.Media;
    public interface IDocumentLineTransformer
    {
        void ColorizeLine(DocumentLine line, FormattedText formattedText);
    }
}
