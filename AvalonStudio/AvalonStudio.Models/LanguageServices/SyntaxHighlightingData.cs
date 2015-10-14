namespace AvalonStudio.TextEditor
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

    public class SyntaxHighlightingData
    {  
        public HighlightType Type { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
