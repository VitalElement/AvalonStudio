using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;
using AvalonStudio.CodeEditor;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;

namespace AvalonStudio.Languages.D
{
    class DKeywordTextHighlightTransformer : GenericLineTransformer
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
