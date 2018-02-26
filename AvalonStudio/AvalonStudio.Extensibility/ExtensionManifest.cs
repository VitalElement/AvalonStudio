using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AvalonStudio.Extensibility
{
    internal class ExtensionManifest : IExtensionManifest
    {
        private const string MefComponentsString = "mefComponents";

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty(Required = Required.Always)]
        public string Version { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        [JsonProperty(Required = Required.Always)]
        public IDictionary<string, IEnumerable<string>> Assets { get; set; }
        
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }

        private string _directory;

        public ExtensionManifest(string manifestPath)
        {
            _directory = Path.GetDirectoryName(manifestPath);
        }

        public IEnumerable<string> GetMefComponents()
        {
            if (Assets.TryGetValue(MefComponentsString, out var assemblies))
            {
                return assemblies.Select(a => Path.Combine(_directory, a));
            }

            return Enumerable.Empty<string>();
        }

        public static IExtensionManifest LoadFromManifest(string extensionManifestPath)
        {
            using (var reader = new StreamReader(extensionManifestPath))
            {
                var extension = new ExtensionManifest(extensionManifestPath);
                JsonSerializer.Create().Populate(reader, extension);

                return extension;
            }
        }
    }
}
