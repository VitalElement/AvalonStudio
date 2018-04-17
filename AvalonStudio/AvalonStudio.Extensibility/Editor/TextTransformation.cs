namespace AvalonStudio.Extensibility.Editor
{
    using Avalonia.Media;
    using AvaloniaEdit.Document;
    using AvalonStudio.CodeEditor;

    public abstract class TextTransformation : TextSegment
    {
        public TextTransformation(object tag, int startOffset, int endOffset)
        {
            Tag = tag;
            StartOffset = startOffset;
            EndOffset = endOffset;
        }

        public abstract void Transform(GenericLineTransformer transformer, DocumentLine line);

        public object Tag { get; }
    }

    public class ForegroundTextTransformation : TextTransformation
    {
        public ForegroundTextTransformation(object tag, int startOffset, int endOffset, IBrush foreground) : base(tag, startOffset, endOffset)
        {
            Foreground = foreground;
        }

        public IBrush Foreground { get; }

        public override void Transform(GenericLineTransformer transformer, DocumentLine line)
        {
            if (Length == 0)
            {
                return;
            }

            var formattedOffset = 0;
            var endOffset = line.EndOffset;

            if (StartOffset > line.Offset)
            {
                formattedOffset = StartOffset - line.Offset;
            }

            if(EndOffset < line.EndOffset)
            {
                endOffset = EndOffset;
            }

            transformer.SetTextStyle(line, formattedOffset, endOffset - line.Offset - formattedOffset, Foreground);
        }
    }

    public class OpacityTextTransformation : TextTransformation
    {
        public OpacityTextTransformation(object tag, int startOffset, int endOffset, double opacity) : base(tag, startOffset, endOffset)
        {
            Opacity = opacity;
        }

        public double Opacity { get; }

        public override void Transform(GenericLineTransformer transformer, DocumentLine line)
        {
            if(Length == 0)
            {
                return;
            }

            var formattedOffset = 0;
            var endOffset = line.EndOffset;

            if (StartOffset > line.Offset)
            {
                formattedOffset = StartOffset - line.Offset;
            }

            if (EndOffset < line.EndOffset)
            {
                endOffset = EndOffset;
            }

            transformer.SetTextOpacity(line, formattedOffset, endOffset - line.Offset - formattedOffset, Opacity);            
        }
    }
}
