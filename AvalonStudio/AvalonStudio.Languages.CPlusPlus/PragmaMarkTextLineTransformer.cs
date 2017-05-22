using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.CodeEditor;

namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    internal class PragmaMarkTextLineTransformer : GenericLineTransformer
    {
        private readonly SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(0xD0, 0xB8, 0x48, 0xFF));
        private readonly SolidColorBrush pragmaBrush = new SolidColorBrush(Color.FromArgb(0x88, 0xB8, 0x48, 0xFF));

        protected override void TransformLine(DocumentLine line, ITextRunConstructionContext context)
        {
            var text = context.Document.GetText(line);

            if (!text.Trim().StartsWith("//"))
            {
                if (text.Contains("#pragma mark"))
                {
                    var startIndex = text.IndexOf("#pragma mark");

                    SetTextStyle(line, startIndex, 12, pragmaBrush);
                    SetTextStyle(line, startIndex + 12, text.Length - 12, brush);
                }
                else if (text.Contains("#pragma"))
                {
                    var startIndex = text.IndexOf("#pragma");

                    SetTextStyle(line, startIndex, 7, pragmaBrush);
                }
            }
        }
    }
}