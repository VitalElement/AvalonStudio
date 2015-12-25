namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Perspex.Media;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class VisualLine : ISegment
    {
        public DocumentLine DocumentLine { get; set; }
        public UInt32 VisualLineNumber { get; set; }
        public FormattedText RenderedText { get; set; }

        public int Offset
        {
            get
            {
                return DocumentLine.Offset;
            }
        }

        public int Length
        {
            get
            {
                return DocumentLine.Length;
            }
        }

        public int EndOffset
        {
            get
            {
                return DocumentLine.EndOffset;
            }
        }
    }
}
