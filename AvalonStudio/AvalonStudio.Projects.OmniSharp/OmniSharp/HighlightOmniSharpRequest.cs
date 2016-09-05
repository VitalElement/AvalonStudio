using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

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

    class HighlightOmniSharpRequest : OmniSharpRequest<OmniSharpHighlightData>
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
