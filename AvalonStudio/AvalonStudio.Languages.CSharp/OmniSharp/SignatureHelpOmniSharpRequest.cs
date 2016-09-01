using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    class SignatureHelpOmniSharpRequest : OmniSharpRequest<SignatureHelpResponseData>
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
