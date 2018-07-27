using System;
using System.Collections.Generic;
using System.Composition;

namespace AvalonStudio.Languages
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportLanguageServiceProviderAttribute : ExportAttribute
    {
        public IEnumerable<string> TargetCapabilities { get; }

        public ExportLanguageServiceProviderAttribute(params string[] targetCapabilities)
            : base (typeof(ILanguageServiceProvider))
        {
            TargetCapabilities = targetCapabilities;
        }
    }
}
