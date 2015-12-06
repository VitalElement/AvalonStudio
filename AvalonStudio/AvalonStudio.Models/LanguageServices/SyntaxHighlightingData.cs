using System;
using System.Collections.Generic;

namespace AvalonStudio.Models.LanguageServices
{        
    public enum HighlightType
    {
        Punctuation,
        Keyword,
        Identifier,
        Literal,
        Comment,
        UserType
    }

    public class SyntaxHighlightDataList : List<SyntaxHighlightingData>
    {
        public new void Add (SyntaxHighlightingData item)
        {
            var index = this.BinarySearch(item);

            if (index < 0)
            {
                this.Insert(~index, item);
            }
            else
            {
                this.Insert(index + 1, item);
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
            if(this.Start > other.Start)
            {
                return 1;
            }

            if (this.Start == other.Start)
            {
                return 0;
            }

            return -1;
        }
    }
}
