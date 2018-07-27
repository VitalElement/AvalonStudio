using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace AvalonStudio.Languages
{
    public static class ContentTypeServiceInstance
    {
        public static IContentTypeService Instance { get; internal set; }
    }

    [Export(typeof(IContentTypeService))]
    [Shared]
    internal class ContentTypeService : IContentTypeService
    {
        public IReadOnlyDictionary<string, IContentType> ContentTypes => _contentTypes.Value;

        private IEnumerable<Lazy<object, ContentTypeMetadata>> _contentTypeImports;
        private Lazy<IReadOnlyDictionary<string, IContentType>> _contentTypes;
        
        [ImportingConstructor]
        public ContentTypeService(
            [ImportMany(ExportContractNames.ContentType)] IEnumerable<Lazy<object, ContentTypeMetadata>> contentTypes)
        {
            _contentTypeImports = contentTypes;
            _contentTypes = new Lazy<IReadOnlyDictionary<string, IContentType>>(GroupContentTypes);

            ContentTypeServiceInstance.Instance = this;
        }

        public bool CapabilityAppliesToContentType(string capability, string contentType)
        {
            if (ContentTypes.TryGetValue(contentType, out var type))
            {
                return type.Capabilities.Any(c => OrdinalIgnoreCaseEquals(c, capability));
            }

            return false;
        }

        public string GetContentTypeForExtension(string extension)
        {
            extension = extension.TrimStart('.');

            var contentType = ContentTypes.FirstOrDefault(
                t => t.Value.Extensions.Any(
                    e => OrdinalIgnoreCaseEquals(e.TrimStart('.'), extension))).Key;

            if (String.IsNullOrEmpty(contentType))
            {
                return "Plain";
            }

            return contentType;
        }

        private IReadOnlyDictionary<string, IContentType> GroupContentTypes()
        {
            var dictionary = new Dictionary<string, IContentType>(StringComparer.OrdinalIgnoreCase);

            foreach (var contentType in _contentTypeImports.Select(t => t.Metadata))
            {
                var name = contentType.Name;

                if (!dictionary.ContainsKey(name))
                {
                    dictionary.Add(name, contentType);
                }
            }

            return dictionary;
        }

        private static bool OrdinalIgnoreCaseEquals(string a, string b) =>
            String.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    }
}
