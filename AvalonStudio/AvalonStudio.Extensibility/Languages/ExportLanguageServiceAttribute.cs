using System;
using System.Composition;

namespace AvalonStudio.Languages
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExportLanguageServiceAttribute : ExportAttribute, ILanguageServiceMetadata
    {
        public string ContentType { get; }

        public ExportLanguageServiceAttribute(string contentType)
            : base (typeof(ILanguageService))
        {
            ContentType = contentType;
        }
    }
}
