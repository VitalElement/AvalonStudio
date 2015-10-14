namespace AvalonStudio.TextEditor
{    
    public class Colour
    {
        public int R;
        public int G;
        public int B;
    }

    public class SyntaxHighlightingData
    {        
        public Colour Foreground { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
