using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.CodeEditor;
using AvalonStudio.Utils;
using System.Collections.Generic;

namespace AvalonStudio.Debugging
{
    class DisassemblyViewTextColorizer : GenericLineTransformer
    {
        private Dictionary<string, int> _addressLines;
        private IBrush _addressBrush = Brush.Parse("#569CD6");
        private IBrush _operationBrush = Brush.Parse("#4EC9B0");

        public DisassemblyViewTextColorizer(Dictionary<string, int> addressLines)
        {
            _addressLines = addressLines;
        }

        protected override void TransformLine(DocumentLine line, ITextRunConstructionContext context)
        {
            if (_addressLines.ContainsValue(line.LineNumber))
            {
                // assembly line
                var lineText = context.Document.GetText(line);

                var firstWhiteSpace = lineText.IndexOf(c => char.IsWhiteSpace(c));

                SetTextStyle(line, 0, firstWhiteSpace, _addressBrush);

                SetTextStyle(line, firstWhiteSpace, lineText.Length - firstWhiteSpace, _operationBrush);
            }
            else
            {
                // source line.
            }
        }
    }
}
