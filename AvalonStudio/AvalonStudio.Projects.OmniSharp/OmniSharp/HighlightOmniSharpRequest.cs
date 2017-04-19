namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    internal class HighlightOmniSharpRequest : OmniSharpRequest<OmniSharpHighlightData>
    {
        public override string EndPoint
        {
            get
            {
                return "highlight";
            }
        }
    }
}