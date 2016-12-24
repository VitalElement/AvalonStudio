using System;
using System.Diagnostics;
using System.Globalization;
using AvalonStudio.TextEditor.Utils;

namespace AvalonStudio.TextEditor.Document
{
	/// <summary>
	///     Represents a simple segment (Offset,Length pair) that is not automatically updated
	///     on document changes.
	/// </summary>
	internal struct SimpleSegment : IEquatable<SimpleSegment>, ISegment
	{
		public static readonly SimpleSegment Invalid = new SimpleSegment(-1, -1);

		/// <summary>
		///     Gets the overlapping portion of the segments.
		///     Returns SimpleSegment.Invalid if the segments don't overlap.
		/// </summary>
		public static SimpleSegment GetOverlap(ISegment segment1, ISegment segment2)
		{
			var start = Math.Max(segment1.Offset, segment2.Offset);
			var end = Math.Min(segment1.EndOffset, segment2.EndOffset);
			if (end < start)
				return Invalid;
			return new SimpleSegment(start, end - start);
		}

		public readonly int Offset, Length;

		int ISegment.Offset
		{
			get { return Offset; }
		}

		int ISegment.Length
		{
			get { return Length; }
		}

		public int EndOffset
		{
			get { return Offset + Length; }
		}

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
				return Offset + 10301*Length;
			}
		}

		public override bool Equals(object obj)
		{
			return obj is SimpleSegment && Equals((SimpleSegment) obj);
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

		/// <inheritdoc />
		public override string ToString()
		{
			return "[Offset=" + Offset.ToString(CultureInfo.InvariantCulture) + ", Length=" +
			       Length.ToString(CultureInfo.InvariantCulture) + "]";
		}
	}

	/// <summary>
	///     A segment using <see cref="TextAnchor" />s as start and end positions.
	/// </summary>
	/// <remarks>
	///     <para>
	///         For the constructors creating new anchors, the start position will be AfterInsertion and the end position will
	///         be BeforeInsertion.
	///         Should the end position move before the start position, the segment will have length 0.
	///     </para>
	/// </remarks>
	/// <seealso cref="ISegment" />
	/// <seealso cref="TextSegment" />
	public sealed class AnchorSegment : ISegment
	{
		private readonly TextAnchor start;
		private readonly TextAnchor end;

		/// <summary>
		///     Creates a new AnchorSegment using the specified anchors.
		///     The anchors must have <see cref="TextAnchor.SurviveDeletion" /> set to true.
		/// </summary>
		public AnchorSegment(TextAnchor start, TextAnchor end)
		{
			if (start == null)
				throw new ArgumentNullException("start");
			if (end == null)
				throw new ArgumentNullException("end");
			if (!start.SurviveDeletion)
				throw new ArgumentException("Anchors for AnchorSegment must use SurviveDeletion", "start");
			if (!end.SurviveDeletion)
				throw new ArgumentException("Anchors for AnchorSegment must use SurviveDeletion", "end");
			this.start = start;
			this.end = end;
		}

		/// <summary>
		///     Creates a new AnchorSegment that creates new anchors.
		/// </summary>
		public AnchorSegment(TextDocument document, ISegment segment)
			: this(document, ThrowUtil.CheckNotNull(segment, "segment").Offset, segment.Length)
		{
		}

		/// <summary>
		///     Creates a new AnchorSegment that creates new anchors.
		/// </summary>
		public AnchorSegment(TextDocument document, int offset, int length)
		{
			if (document == null)
				throw new ArgumentNullException("document");
			start = document.CreateAnchor(offset);
			start.SurviveDeletion = true;
			start.MovementType = AnchorMovementType.AfterInsertion;
			end = document.CreateAnchor(offset + length);
			end.SurviveDeletion = true;
			end.MovementType = AnchorMovementType.BeforeInsertion;
		}

		/// <inheritdoc />
		public int Offset
		{
			get { return start.Offset; }
		}

		/// <inheritdoc />
		public int Length
		{
			get
			{
				// Math.Max takes care of the fact that end.Offset might move before start.Offset.
				return Math.Max(0, end.Offset - start.Offset);
			}
		}

		/// <inheritdoc />
		public int EndOffset
		{
			get
			{
				// Math.Max takes care of the fact that end.Offset might move before start.Offset.
				return Math.Max(start.Offset, end.Offset);
			}
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "[Offset=" + Offset.ToString(CultureInfo.InvariantCulture) + ", EndOffset=" +
			       EndOffset.ToString(CultureInfo.InvariantCulture) + "]";
		}
	}
}