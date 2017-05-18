using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;

namespace AvalonStudio.CodeEditor
{
    public abstract class GenericLineTransformer : DocumentColorizingTransformer
    {
        protected override void ColorizeLine(DocumentLine line)
        {
            TransformLine(line, CurrentContext);
        }

        protected abstract void TransformLine(DocumentLine line, ITextRunConstructionContext context);

        protected void SetTextStyle(DocumentLine line, int startIndex, int length, IBrush foreground)
        {
            if ((line.Offset + startIndex + length) > line.EndOffset)
            {
                length = (line.EndOffset - startIndex) - line.Offset - startIndex;
            }

            ChangeLinePart(line.Offset + startIndex, line.Offset + startIndex + length, e => e.TextRunProperties.ForegroundBrush = foreground);
        }
    }
}
