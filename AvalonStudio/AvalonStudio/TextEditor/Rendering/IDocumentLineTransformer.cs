namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex;
    using Perspex.Media;
    public interface IDocumentLineTransformer
    {
        void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, DocumentLine line, FormattedText formattedText);
    }
}
