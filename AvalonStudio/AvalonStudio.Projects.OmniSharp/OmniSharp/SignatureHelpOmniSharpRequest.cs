using AvalonStudio.Extensibility.Languages.CompletionAssistance;

namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    internal class SignatureHelpOmniSharpRequest : OmniSharpRequest<SignatureHelp>
    {
        public override string EndPoint
        {
            get
            {
                return "signaturehelp";
            }
        }
    }
}