namespace AvalonStudio.TextEditor.Rendering
{
    using Perspex.Media;
    using System;

    public interface IBackgroundRenderer
    {
        void Draw(TextView textView, DrawingContext drawingContext);
        event EventHandler<EventArgs> DataChanged;
    }
}
