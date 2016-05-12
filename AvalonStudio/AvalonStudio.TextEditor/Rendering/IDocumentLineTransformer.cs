namespace AvalonStudio.TextEditor.Rendering
{
    using Avalonia;
    using Avalonia.Media;
    using System;

    public interface IDocumentLineTransformer
    {
        void TransformLine(TextView textView, VisualLine line);
        event EventHandler<EventArgs> DataChanged;         
    }
}
