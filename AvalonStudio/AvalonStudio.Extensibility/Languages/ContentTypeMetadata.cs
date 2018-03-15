using System.Collections.Generic;
using System.ComponentModel;

namespace AvalonStudio.Languages
{
    internal class ContentTypeMetadata : IContentType
    {
        public string Name { get; set; }
        public IEnumerable<string> Capabilities { get; set; }
        [DefaultValue(new string[0])]
        public IEnumerable<string> Extensions { get; set; }
    }
}
