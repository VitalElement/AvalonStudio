using System;
using System.Collections.Generic;

namespace AvalonStudio.Projects
{
    public class ProjectTypeMetadata : IProjectTypeMetadata
    {
        public string Description { get; set; }

        public string DefaultExtension { get; set; }
        public IEnumerable<string> PossibleExtensions { get; set; }

        public Guid ProjectTypeGuid { get; set; }
    }
}
