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
		ClassName,
        StructName,
        PreProcessor,
        PreProcessorText,

	}

	public class SyntaxHighlightDataList : List<OffsetSyntaxHighlightingData>
	{
		public new void Add(OffsetSyntaxHighlightingData item)
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

    public class LineColumnSyntaxHighlightingData : OffsetSyntaxHighlightingData, IComparable<LineColumnSyntaxHighlightingData>
    {
        public int StartColumn { get; set; }
        public int EndColumn { get;set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }

        public int CompareTo(LineColumnSyntaxHighlightingData other)
        {
            if (StartLine > other.StartLine)
            {
                return 1;
            }
            else if (StartLine > other.StartLine && EndLine > other.EndLine)
            {
                return 1;
            }
            else if (StartColumn > other.StartColumn && EndLine > other.EndColumn)
            {
                return 1;
            }

            return -1;
        }
    }
}