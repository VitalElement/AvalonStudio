using System;
using System.Globalization;
using AvalonStudio.TextEditor.Document;

namespace AvalonStudio.TextEditor.Rendering
{
	/// <summary>
	///     Represents a text location with a visual column.
	/// </summary>
	public struct TextViewPosition : IEquatable<TextViewPosition>, IComparable<TextViewPosition>
	{
		/// <summary>
		///     Gets/Sets Location.
		/// </summary>
		public TextLocation Location
		{
			get { return new TextLocation(Line, Column); }
			set
			{
				Line = value.Line;
				Column = value.Column;
			}
		}

		/// <summary>
		///     Gets/Sets the line number.
		/// </summary>
		public int Line { get; set; }

		/// <summary>
		///     Gets/Sets the (text) column number.
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		///     Gets/Sets the visual column number.
		///     Can be -1 (meaning unknown visual column).
		/// </summary>
		public int VisualColumn { get; set; }

		/// <summary>
		///     When word-wrap is enabled and a line is wrapped at a position where there is no space character;
		///     then both the end of the first TextLine and the beginning of the second TextLine
		///     refer to the same position in the document, and also have the same visual column.
		///     In this case, the IsAtEndOfLine property is used to distinguish between the two cases:
		///     the value <c>true</c> indicates that the position refers to the end of the previous TextLine;
		///     the value <c>false</c> indicates that the position refers to the beginning of the next TextLine.
		///     If this position is not at such a wrapping position, the value of this property has no effect.
		/// </summary>
		public bool IsAtEndOfLine { get; set; }

		/// <summary>
		///     Creates a new TextViewPosition instance.
		/// </summary>
		public TextViewPosition(int line, int column, int visualColumn)
		{
			Line = line;
			Column = column;
			VisualColumn = visualColumn;
			IsAtEndOfLine = false;
		}

		/// <summary>
		///     Creates a new TextViewPosition instance.
		/// </summary>
		public TextViewPosition(int line, int column)
			: this(line, column, -1)
		{
		}

		/// <summary>
		///     Creates a new TextViewPosition instance.
		/// </summary>
		public TextViewPosition(TextLocation location, int visualColumn)
		{
			Line = location.Line;
			Column = location.Column;
			VisualColumn = visualColumn;
			IsAtEndOfLine = false;
		}

		/// <summary>
		///     Creates a new TextViewPosition instance.
		/// </summary>
		public TextViewPosition(TextLocation location)
			: this(location, -1)
		{
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture,
				"[TextViewPosition Line={0} Column={1} VisualColumn={2} IsAtEndOfLine={3}]",
				Line, Column, VisualColumn, IsAtEndOfLine);
		}

		#region Equals and GetHashCode implementation

		// The code in this region is useful if you want to use this structure in collections.
		// If you don't need it, you can just remove the region and the ": IEquatable<Struct1>" declaration.

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (obj is TextViewPosition)
				return Equals((TextViewPosition) obj); // use Equals method below
			return false;
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			var hashCode = IsAtEndOfLine ? 115817 : 0;
			unchecked
			{
				hashCode += 1000000007*Line.GetHashCode();
				hashCode += 1000000009*Column.GetHashCode();
				hashCode += 1000000021*VisualColumn.GetHashCode();
			}
			return hashCode;
		}

		/// <summary>
		///     Equality test.
		/// </summary>
		public bool Equals(TextViewPosition other)
		{
			return Line == other.Line && Column == other.Column && VisualColumn == other.VisualColumn &&
			       IsAtEndOfLine == other.IsAtEndOfLine;
		}

		/// <summary>
		///     Equality test.
		/// </summary>
		public static bool operator ==(TextViewPosition left, TextViewPosition right)
		{
			return left.Equals(right);
		}

		/// <summary>
		///     Inequality test.
		/// </summary>
		public static bool operator !=(TextViewPosition left, TextViewPosition right)
		{
			return !left.Equals(right); // use operator == and negate result
		}

		#endregion

		/// <inheritdoc />
		public int CompareTo(TextViewPosition other)
		{
			var r = Location.CompareTo(other.Location);
			if (r != 0)
				return r;
			r = VisualColumn.CompareTo(other.VisualColumn);
			if (r != 0)
				return r;
			if (IsAtEndOfLine && !other.IsAtEndOfLine)
				return -1;
			if (!IsAtEndOfLine && other.IsAtEndOfLine)
				return 1;
			return 0;
		}
	}
}