using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public class Parameter
    {
        public string Name { get; set; }
        public string BuiltInType { get; set; }
        public string Type { get; set; }
        public string Label { get; set; }
        public string Documentation { get; set; }
    }

    public class Signature
    {
        public Signature()
        {
            Parameters = new List<Parameter>();
        }

        public string Name { get; set; }
        public string BuiltInReturnType { get; set; }
        public string ReturnType { get; set; }
        public string Label { get; set; }
        public string Documentation { get; set; }
        public string Description { get; set; }
        public List<Parameter> Parameters { get; set; }
    }

    public class SignatureHelp
    {
        public SignatureHelp()
        {
            Signatures = new List<Signature>();
        }

        public SignatureHelp(List<Signature> signatures, int offset)
        {
            Signatures = signatures;
            Offset = offset;
        }

        public List<Signature> Signatures { get; set; }
        public int ActiveSignature { get; set; }
        public int ActiveParameter { get; set; }
        public int Offset { get; set; }
    }
}