using System;

namespace AvalonStudio.Languages
{
    public class LineColumnSyntaxHighlightingData : OffsetSyntaxHighlightingData, IComparable<LineColumnSyntaxHighlightingData>
    {
        public int StartColumn { get; set; }
        public int EndColumn { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }

        public int CompareTo(LineColumnSyntaxHighlightingData other)
        {
            if (StartLine > other.StartLine)
            {
                return 1;
            }
            else if (StartLine > other.StartLine && EndLine > other.EndLine)
            {
                return 1;
            }
            else if (StartColumn > other.StartColumn && EndLine > other.EndColumn)
            {
                return 1;
            }

            return -1;
        }
    }
}