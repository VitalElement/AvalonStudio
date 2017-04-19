namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    internal class CheckReadyStatusRequest : OmniSharpRequest<bool>
    {
        public override string EndPoint
        {
            get
            {
                return "checkreadystatus";
            }
        }
    }
}