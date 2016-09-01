using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Documentation { get; set; }
    }

    public class Signature
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Documentation { get; set; }
        public List<object> Parameters { get; set; }
    }

    public class SignatureHelpResponseData
    {
        public List<Signature> Signatures { get; set; }
        public int ActiveSignature { get; set; }
        public int ActiveParameter { get; set; }
    }
}
