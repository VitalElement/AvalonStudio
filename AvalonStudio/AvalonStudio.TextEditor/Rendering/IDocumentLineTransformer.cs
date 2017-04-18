using System;

namespace AvalonStudio.TextEditor.Rendering
{
    public interface IDocumentLineTransformer
    {
        void TransformLine(TextView textView, VisualLine line);

        event EventHandler<EventArgs> DataChanged;
    }
}