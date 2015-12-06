namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex.Media;
    public interface IDocumentLineTransformer
    {
        void TransformLine(DocumentLine line, FormattedText formattedText);
    }
}
