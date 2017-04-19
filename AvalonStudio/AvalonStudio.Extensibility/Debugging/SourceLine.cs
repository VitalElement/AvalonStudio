using System.Linq;

namespace AvalonStudio.Debugging
{
    public class SourceLine : DisassembledLine
    {
        private string lineText;

        public string LineText
        {
            get
            {
                if (lineText == null)
                {
                    lineText = System.IO.File.ReadLines(File).Skip(Line - 1).Take(1).First();
                }

                return lineText;
            }
        }

        public int Line { get; set; }
        public string File { get; set; }
        public string FullFileName { get; set; }
    }
}