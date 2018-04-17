using System;
using System.Collections.Generic;
using System.Composition;

namespace AvalonStudio.Languages
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportContentTypeAttribute : ExportAttribute
    {
        public string Name { get; }
        public IEnumerable<string> Capabilities { get; }

        public ExportContentTypeAttribute(string name, params string[] capabilities)
            : base(ExportContractNames.ContentType, typeof(object))
        {
            Name = name;
            Capabilities = capabilities;
        }
    }
}
