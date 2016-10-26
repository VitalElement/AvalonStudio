using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
using AvalonStudio.TextEditor.Rendering;

namespace AvalonStudio.Languages.D
{
    class DKeywordTextHighlightTransformer : IDocumentLineTransformer
    {
        private List<string> keywords = new List<string>() {"new", "this"};
        private IBrush KeywordBrush = Brush.Parse("#569CD6");

        public void TransformLine(TextView textView, VisualLine line)
        {
            

        }

        public event EventHandler<EventArgs> DataChanged;
    }    
}
