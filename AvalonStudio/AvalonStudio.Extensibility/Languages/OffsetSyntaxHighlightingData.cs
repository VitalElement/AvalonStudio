using System;

namespace AvalonStudio.Languages
{
    public class OffsetSyntaxHighlightingData : IComparable<OffsetSyntaxHighlightingData>
    {
        public HighlightType Type { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        public int CompareTo(OffsetSyntaxHighlightingData other)
        {
            if (Start > other.Start)
            {
                return 1;
            }

            if (Start == other.Start)
            {
                return 0;
            }

            return -1;
        }
    }
}