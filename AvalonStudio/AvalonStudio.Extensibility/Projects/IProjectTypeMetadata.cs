using System;
using System.Collections.Generic;

namespace AvalonStudio.Projects
{
    public interface IProjectTypeMetadata
    {
        string Description { get; }

        string DefaultExtension { get; }
        IEnumerable<string> PossibleExtensions { get; }

        Guid ProjectTypeGuid { get; }
    }
}
