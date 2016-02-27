namespace AvalonStudio.TextEditor.Rendering
{
    using Perspex.Media;

    public interface IBackgroundRenderer
    {
        void Draw(TextView textView, DrawingContext drawingContext);
    }
}
