namespace AvalonStudio.Extensibility.Editor
{
    using Avalonia.Media;
    using AvaloniaEdit.Document;

    public abstract class TextTransformation : TextSegment
    {
        public TextTransformation(object tag, int startOffset, int endOffset)
        {
            Tag = tag;
            StartOffset = startOffset;
            EndOffset = endOffset;
        }

        public object Tag { get; }
    }

    public class ForegroundTextTransformation : TextTransformation
    {
        public ForegroundTextTransformation(object tag, int startOffset, int endOffset, IBrush foreground) : base(tag, startOffset, endOffset)
        {
            Foreground = foreground;
        }

        public IBrush Foreground { get; }
    }

    public class OpacityTextTransformation : TextTransformation
    {
        public OpacityTextTransformation(object tag, int startOffset, int endOffset, double opacity) : base(tag, startOffset, endOffset)
        {
            Opacity = opacity;
        }

        public double Opacity { get; }
    }
}
