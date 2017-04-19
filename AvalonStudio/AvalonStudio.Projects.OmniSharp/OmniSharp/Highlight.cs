using System.Collections.Generic;

namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    public class Highlight
    {
        public int StartLine { get; set; }
        public int StartColumn { get; set; }
        public int EndLine { get; set; }
        public int EndColumn { get; set; }
        public string Kind { get; set; }
        public List<string> Projects { get; set; }
    }
}