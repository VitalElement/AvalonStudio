using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
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