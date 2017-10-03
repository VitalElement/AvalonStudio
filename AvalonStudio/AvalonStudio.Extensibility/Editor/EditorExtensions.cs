using AvaloniaEdit.Document;

namespace AvalonStudio.CodeEditor
{
    public static class EditorExtensions
    {
        public static void TrimTrailingWhiteSpace(this TextDocument document, ISegment line)
        {
            document.Replace(line, document.GetText(line).TrimEnd());
        }

        public static void TrimTrailingWhiteSpace(this TextDocument document, int lineNumber)
        {
            var line = document.GetLineByNumber(lineNumber);
            document.TrimTrailingWhiteSpace(line);            
        }        

        public static void TrimTrailingWhiteSpace(this TextDocument document)
        {
            using (document.RunUpdate())
            {
                foreach (var line in document.Lines)
                {
                    document.Replace(line, document.GetText(line).TrimEnd());
                }
            }
        }
    }
}
