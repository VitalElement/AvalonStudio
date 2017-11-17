using System;
using System.Diagnostics;

namespace AvalonStudio.Documents
{
    public struct SimpleSegment : IEquatable<SimpleSegment>, ISegment
    {
        public static readonly SimpleSegment Invalid = new SimpleSegment(-1, -1);

        /// <summary>
        /// Gets the overlapping portion of the segments.
        /// Returns SimpleSegment.Invalid if the segments don't overlap.
        /// </summary>
        public static SimpleSegment GetOverlap(ISegment segment1, ISegment segment2)
        {
            var start = Math.Max(segment1.Offset, segment2.Offset);
            var end = Math.Min(segment1.EndOffset, segment2.EndOffset);
            return end < start ? Invalid : new SimpleSegment(start, end - start);
        }

        public readonly int Offset, Length;

        int ISegment.Offset => Offset;

        int ISegment.Length => Length;

        public int EndOffset => Offset + Length;

        public SimpleSegment(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }

        public SimpleSegment(ISegment segment)
        {
            Debug.Assert(segment != null);
            Offset = segment.Offset;
            Length = segment.Length;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Offset + 10301 * Length;
            }
        }

        public override bool Equals(object obj)
        {
            return (obj is SimpleSegment) && Equals((SimpleSegment)obj);
        }

        public bool Equals(SimpleSegment other)
        {
            return Offset == other.Offset && Length == other.Length;
        }

        public static bool operator ==(SimpleSegment left, SimpleSegment right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SimpleSegment left, SimpleSegment right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[Offset={Offset}, Length={Length}]";
        }
    }
}
