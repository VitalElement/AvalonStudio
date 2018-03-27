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

        /// <summary>
        /// Gets whether <paramref name="segment"/> fully contains the specified segment.
        /// </summary>
        /// <remarks>
        /// Use <c>segment.Contains(offset, 0)</c> to detect whether a segment (end inclusive) contains offset;
        /// use <c>segment.Contains(offset, 1)</c> to detect whether a segment (end exclusive) contains offset.
        /// </remarks>
        public bool Contains(int offset, int length)
        {
            return StartOffset <= offset && offset + length < EndOffset;
        }

        /// <summary>
        /// Gets whether <paramref name="thisSegment"/> fully contains the specified segment.
        /// </summary>
        public bool Contains(ISegment segment)
        {
            return StartOffset <= segment.Offset && segment.EndOffset <= EndOffset;
        }

        public string Spelling { get; set; }

        public CursorKind Kind { get; set; }

        public int CompareTo(IndexEntry other)
        {
            if(other.StartOffset <= StartOffset && other.EndOffset >= EndOffset)
            {
                return Length.CompareTo(other.Length);
            }
            else
            {
                return StartOffset.CompareTo(other.StartOffset);
            }
        }
    }
}