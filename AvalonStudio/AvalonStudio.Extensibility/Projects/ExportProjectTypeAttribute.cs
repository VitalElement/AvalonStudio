using System;
using System.Collections.Generic;
using System.Composition;

namespace AvalonStudio.Projects
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportProjectTypeAttribute : ExportAttribute, IProjectTypeMetadata
    {
        public string Description { get; }

        public string DefaultExtension { get; }
        public IEnumerable<string> PossibleExtensions { get; }

        public Guid ProjectTypeGuid { get; }

        public ExportProjectTypeAttribute(
            string defaultExtension,
            string description = null,
            string projectTypeGuid = null,
            params string[] possibleExtensions)
            : base(typeof(IProjectType))
        {
            Description = description ?? "Unknown Project";

            DefaultExtension = defaultExtension;
            PossibleExtensions = possibleExtensions;

            if (projectTypeGuid != null)
            {
                ProjectTypeGuid = Guid.Parse(projectTypeGuid);
            }
        }
    }
}
