namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    public class WorkspaceInformationRequest : OmniSharpRequest<Workspace>
    {
        public bool ExcludeSourceFiles { get; set; }

        public override string EndPoint
        {
            get
            {
                return "projects";
            }
        }
    }
}