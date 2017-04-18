using Avalonia.Media;
using System;

namespace AvalonStudio.TextEditor.Rendering
{
    public interface IBackgroundRenderer
    {
        void Draw(TextView textView, DrawingContext drawingContext);

        void TransformLine(TextView textView, DrawingContext drawingContext, VisualLine line);

        event EventHandler<EventArgs> DataChanged;
    }
}