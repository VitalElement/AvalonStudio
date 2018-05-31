using System.Collections.Generic;
using System.ComponentModel;

namespace AvalonStudio.Commands
{
    public class CommandDefinitionMetadata
    {
        public string Name { get; set; }

        [DefaultValue(null)]
        public IEnumerable<string> DefaultKeyGestures { get; set; }
    }
}
