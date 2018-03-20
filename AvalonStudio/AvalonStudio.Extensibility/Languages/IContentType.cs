using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public interface IContentType
    {
        string Name { get; set; }
        IEnumerable<string> Extensions { get; set; }
        IEnumerable<string> Capabilities { get; set; }
    }
}
