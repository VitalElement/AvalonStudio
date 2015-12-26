namespace AvalonStudio.TextEditor.Rendering
{    
    using Perspex;
    using Perspex.Media;

    public interface IDocumentLineTransformer
    {
        void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, VisualLine line);
    }
}
