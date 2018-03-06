using System;
using System.Collections.Generic;
using System.Composition;

namespace AvalonStudio.Projects
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportSolutionTypeAttribute : ExportAttribute, ISolutionTypeMetadata
    {
        public IEnumerable<string> SupportedExtensions { get; }

        public ExportSolutionTypeAttribute(params string[] supportedExtensions)
            : base (typeof(ISolutionType))
        {
            SupportedExtensions = supportedExtensions;
        }
    }
}
