namespace AvalonStudio.TextEditor
{    
    public class Colour
    {
        public Colour(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R;
        public byte G;
        public byte B;
    }

    public class SyntaxHighlightingData
    {  
        public Colour Foreground { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }
}
