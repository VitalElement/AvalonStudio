using Microsoft.CodeAnalysis.PasteTracking;
using Microsoft.CodeAnalysis.Text;
using System.Composition;

namespace AvalonStudio.Projects.OmniSharp.Roslyn
{
    [Export(typeof(IPasteTrackingService)), Shared]
    internal class PasteTrackingService : IPasteTrackingService
    {
        public bool TryGetPastedTextSpan(SourceTextContainer sourceTextContainer, out TextSpan textSpan)
        {
            textSpan = default;
            return false;
        }
    }
}
