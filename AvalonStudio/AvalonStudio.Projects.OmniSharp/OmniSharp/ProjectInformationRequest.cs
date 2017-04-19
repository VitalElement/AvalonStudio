using RestSharp;

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