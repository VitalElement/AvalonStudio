using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public interface IContentTypeService
    {
        IReadOnlyDictionary<string, IContentType> ContentTypes { get; }

        bool CapabilityAppliesToContentType(string capability, string contentType);
        string GetContentTypeForExtension(string extension);
    }
}
