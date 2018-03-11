namespace AvalonStudio.Extensibility.Editor
{
    using Avalonia.Media;
    using AvaloniaEdit.Document;

    public class TextTransformation : TextSegment
    {
        public object Tag { get; set; }
        public IBrush Foreground { get; set; }
        public double Opacity { get; set; } = 1;
    }
}
