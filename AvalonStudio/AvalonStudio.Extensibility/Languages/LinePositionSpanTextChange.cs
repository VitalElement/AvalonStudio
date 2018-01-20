using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Languages
{
    public class LinePositionSpanTextChange
    {
        public string NewText { get; set; }
        
        public int StartLine { get; set; }        
        public int StartColumn { get; set; }        
        public int EndLine { get; set; }        
        public int EndColumn { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as LinePositionSpanTextChange;
            if (other == null)
            {
                return false;
            }

            return NewText == other.NewText
                && StartLine == other.StartLine
                && StartColumn == other.StartColumn
                && EndLine == other.EndLine
                && EndColumn == other.EndColumn;
        }

        public override int GetHashCode()
        {
            return NewText.GetHashCode()
                * (23 + StartLine)
                * (29 + StartColumn)
                * (31 + EndLine)
                * (37 + EndColumn);
        }

        public override string ToString()
        {
            var displayText = NewText != null
                ? NewText.Replace("\r", @"\r").Replace("\n", @"\n").Replace("\t", @"\t")
                : string.Empty;

            return $"StartLine={StartLine}, StartColumn={StartColumn}, EndLine={EndLine}, EndColumn={EndColumn}, NewText='{displayText}'";
        }
    }

}
