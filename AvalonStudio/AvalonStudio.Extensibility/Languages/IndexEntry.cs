using AvaloniaEdit.Document;
using AvalonStudio.Languages;
using System;

namespace AvalonStudio.Extensibility.Languages
{
    public class IndexEntry : TextSegment, IComparable<IndexEntry>
    {
        public IndexEntry(string spelling, int offset, int endOffset, CursorKind kind)
        {
            Spelling = spelling;
            Kind = kind;
            StartOffset = offset;
            EndOffset = endOffset;
        }

        public string Spelling { get; set; }

        public CursorKind Kind { get; set; }

        public int CompareTo(IndexEntry other)
        {
            if (StartOffset == other.StartOffset)
            {
                return EndOffset.CompareTo(other.EndOffset);
            }

            return StartOffset.CompareTo(other.StartOffset);
        }
    }
}