using System.Collections.Generic;

namespace AvalonStudio.Projects
{
    public class SolutionTypeMetadata : ISolutionTypeMetadata
    {
        public IEnumerable<string> SupportedExtensions { get; set; }
    }
}
