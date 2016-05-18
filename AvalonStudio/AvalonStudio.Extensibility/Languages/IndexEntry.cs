﻿namespace AvalonStudio.Extensibility.Languages
{
    using AvalonStudio.Languages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IndexEntry : IComparable<IndexEntry>
    {
        public IndexEntry(string spelling, int offset, int endOffset, CursorKind kind)
        {
            Spelling = spelling;
            Kind = kind;             
            this.Offset = offset;
            this.EndOffset = endOffset;
        }

        public int Line { get; set; }
        public int Column { get; set; }
        public int Length { get; set; }
        public string Spelling { get; set; }

        public CursorKind Kind { get; set; }

        public int Offset { get; set; }   // These are to be converted to line / column numbers when rendering.
        public int EndOffset { get; set; }

        public int CompareTo(IndexEntry other)
        {
            if (this.Offset == other.Offset)
            {
                return this.EndOffset.CompareTo(other.EndOffset);
            }

            return this.Offset.CompareTo(other.Offset);
        }
    }
}
