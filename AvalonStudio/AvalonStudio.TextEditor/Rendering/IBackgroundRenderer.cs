namespace AvalonStudio.TextEditor.Rendering
{
    using Perspex.Media;
    using System;

    public interface IBackgroundRenderer
    {
        void Draw(TextView textView, DrawingContext drawingContext);
        void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line);

        event EventHandler<EventArgs> DataChanged;
    }
}
