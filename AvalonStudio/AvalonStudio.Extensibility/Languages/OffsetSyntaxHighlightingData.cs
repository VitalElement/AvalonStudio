using AvalonStudio.Documents;
using System;

namespace AvalonStudio.Languages
{
    public class OffsetSyntaxHighlightingData : IComparable<OffsetSyntaxHighlightingData>, ISegment
    {
        public OffsetSyntaxHighlightingData()
        {

        }

        public OffsetSyntaxHighlightingData(ISegment segment)
        {
            Offset = segment.Offset;
            Length = segment.Length;
        }

        public OffsetSyntaxHighlightingData (int offset, int length)
        {
            Offset = offset;
            Length = length;
        }

        public HighlightType Type { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
        public int EndOffset => Offset + Length;

        public int CompareTo(OffsetSyntaxHighlightingData other)
        {
            if (Offset > other.Offset)
            {
                return 1;
            }

            if (Offset == other.Offset)
            {
                return 0;
            }

            return -1;
        }
    }
}