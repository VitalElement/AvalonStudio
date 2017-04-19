using AvalonStudio.Languages;
using System;

namespace AvalonStudio.Extensibility.Languages
{
    public class IndexEntry : IComparable<IndexEntry>
    {
        public IndexEntry(string spelling, int offset, int endOffset, CursorKind kind)
        {
            Spelling = spelling;
            Kind = kind;
            Offset = offset;
            EndOffset = endOffset;
        }

        public int Line { get; set; }
        public int Column { get; set; }
        public int Length { get; set; }
        public string Spelling { get; set; }

        public CursorKind Kind { get; set; }

        public int Offset { get; set; } // These are to be converted to line / column numbers when rendering.
        public int EndOffset { get; set; }

        public int CompareTo(IndexEntry other)
        {
            if (Offset == other.Offset)
            {
                return EndOffset.CompareTo(other.EndOffset);
            }

            return Offset.CompareTo(other.Offset);
        }
    }
}