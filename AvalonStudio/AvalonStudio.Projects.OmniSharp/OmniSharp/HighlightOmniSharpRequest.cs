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

    public class OmniSharpHighlightData
    {
        public List<Highlight> Highlights { get; set; }
    }

    internal class HighlightOmniSharpRequest : OmniSharpRequest<OmniSharpHighlightData>
    {
        public override string EndPoint
        {
            get
            {
                return "highlight";
            }
        }
    }
}