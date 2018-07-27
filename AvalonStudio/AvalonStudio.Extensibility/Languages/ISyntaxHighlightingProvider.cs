using AvalonStudio.Documents;
using AvalonStudio.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Languages
{
    public interface ISyntaxHighlightingProvider
    {
        List<OffsetSyntaxHighlightingData> GetHighlightedLine (ISegment line);
    }
}
