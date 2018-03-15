using System.Collections.Generic;

namespace AvalonStudio.Projects
{
    public interface ISolutionTypeMetadata
    {
        IEnumerable<string> SupportedExtensions { get; }
    }
}
