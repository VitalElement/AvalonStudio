namespace AvalonStudio.TextEditor.Rendering
{
    using Perspex.Media;
    using AvalonStudio.TextEditor.Rendering;

    public interface IBackgroundRenderer
    {
        void Draw(TextView textView, DrawingContext drawingContext);
    }
}
