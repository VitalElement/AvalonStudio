using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    public class ProjectInformationRequest : OmniSharpRequest<RestRequest>
    {
        public override string EndPoint
        {
            get
            {
                return "project";
            }
        }
    }
}
