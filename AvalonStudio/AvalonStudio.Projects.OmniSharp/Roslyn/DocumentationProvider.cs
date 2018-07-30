using AvalonStudio.Extensibility;
using Microsoft.CodeAnalysis;
using System.Collections.Concurrent;
using System.Composition;
using System.IO;

namespace AvalonStudio.Projects.OmniSharp.Roslyn
{
    [Export(typeof(DocumentationProvider))]
    [Shared]
    public class DocumentationProvider : IActivatableExtension
    {
        private readonly ConcurrentDictionary<string, Microsoft.CodeAnalysis.DocumentationProvider> _assemblyPathToDocumentationProviderMap = new ConcurrentDictionary<string, Microsoft.CodeAnalysis.DocumentationProvider>();

        public void Activation()
        {
        }

        public void BeforeActivation()
        {            
        }

        public Microsoft.CodeAnalysis.DocumentationProvider GetDocumentationProvider(string location)
        {
            var finalPath = Path.ChangeExtension(location, "xml");

            if (!File.Exists(finalPath))
            {
                return null;
            }

            return _assemblyPathToDocumentationProviderMap.GetOrAdd(location,
                _ => finalPath == null ? null : XmlDocumentationProvider.CreateFromFile(finalPath));
        }
    }
}
