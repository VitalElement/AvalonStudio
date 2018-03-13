using System;
using System.Collections.Generic;
using System.Composition;

namespace AvalonStudio.Languages
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportLanguageServiceAttribute : ExportAttribute
    {
        public IEnumerable<string> TargetCapabilities { get; }

        public ExportLanguageServiceAttribute(params string[] targetCapabilities)
            : base (typeof(ILanguageService))
        {
            TargetCapabilities = targetCapabilities;
        }
    }
}
