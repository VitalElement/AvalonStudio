using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public class SignatureHelp
    {
        public SignatureHelp(int offset)
        {
            Signatures = new List<Signature>();
            Offset = offset;
        }

        public SignatureHelp(List<Signature> signatures, int offset)
        {
            Signatures = signatures;
            Offset = offset;
        }

        public List<Signature> Signatures { get; set; }
        public int ActiveSignature { get; set; }
        public int ActiveParameter { get; set; }
        public int Offset { get; }
    }
}