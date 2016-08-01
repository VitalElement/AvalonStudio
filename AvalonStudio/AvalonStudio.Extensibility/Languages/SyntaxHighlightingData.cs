using System;
using System.Collections.Generic;

namespace AvalonStudio.Languages
{
	public enum HighlightType
	{
        None,
		CallExpression,
		Punctuation,
		Keyword,
		Identifier,
		Literal,
		Comment,
		UserType
	}

	public class SyntaxHighlightDataList : List<SyntaxHighlightingData>
	{
		public new void Add(SyntaxHighlightingData item)
		{
			var index = BinarySearch(item);

			if (index < 0)
			{
				Insert(~index, item);
			}
			else
			{
				Insert(index + 1, item);
			}
		}
	}

	public class SyntaxHighlightingData : IComparable<SyntaxHighlightingData>
	{
		public HighlightType Type { get; set; }
		public int Start { get; set; }
		public int Length { get; set; }

		public int CompareTo(SyntaxHighlightingData other)
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