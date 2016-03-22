namespace AvalonStudio.TextEditor.Rendering
{
    using Perspex;
    using Perspex.Media;
    using System;

    public interface IDocumentLineTransformer
    {
        void TransformLine(TextView textView, VisualLine line);
        event EventHandler<EventArgs> DataChanged;         
    }
}
