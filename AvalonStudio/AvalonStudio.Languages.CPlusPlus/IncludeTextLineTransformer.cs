using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.CodeEditor;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    internal class IncludeTextLineTransformer : GenericLineTransformer
    {
        private readonly IBrush brush = Brush.Parse("#D69D85");
        private readonly IBrush pragmaBrush = Brush.Parse("#9B9B9B");

        protected override void TransformLine(DocumentLine line, ITextRunConstructionContext context)
        {
            var text = context.Document.GetText(line);
            if (text.Contains("#include") && !text.Trim().StartsWith("//"))
            {
                var startIndex = text.IndexOf("#include");

                SetTextStyle(line, startIndex, 8, pragmaBrush);
                SetTextStyle(line, startIndex + 8, text.Length - (startIndex + 8), brush);
            }
        }
    }
}