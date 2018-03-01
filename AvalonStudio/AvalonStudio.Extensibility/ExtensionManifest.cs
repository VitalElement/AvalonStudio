using AvalonStudio.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace AvalonStudio.Extensibility
{
    internal class ExtensionManifest : IExtensionManifest
    {
        private static string DefaultIcon = "resm:AvalonStudio.Assets.logo-256.png?assembly=AvalonStudio";

        [JsonProperty(Required = Required.Always)]
        public string Name { get; set; }
        [JsonProperty(Required = Required.Always)]
        public Version Version { get; set; }
        public string Description { get; set; }
        public string Icon
        {
            get => File.Exists(_icon) ? _icon : DefaultIcon;
            set => _icon = GetFullPath(value);
        }

        [JsonProperty(Required = Required.Always)]
        public IReadOnlyDictionary<string, IEnumerable<string>> Assets { get; }

        [JsonIgnore]
        public IReadOnlyDictionary<string, object> AdditionalData => _additionalData;
        
        private string _directory;

        private string _icon;

        [JsonExtensionData]
        private Dictionary<string, object> _additionalData;

        public ExtensionManifest(string manifestPath)
        {
            _directory = Path.GetDirectoryName(manifestPath);
            _additionalData = new Dictionary<string, object>();

            Assets = new Dictionary<string, IEnumerable<string>>();

            using (var reader = new StreamReader(manifestPath))
            {
                SerializedObject.PopulateObject(reader, this);
            }
        }

        private string GetFullPath(string relativePath)
        {
            if (relativePath == null)
            {
                return relativePath;
            }

            return Path.GetFullPath(Path.Combine(_directory, relativePath));
        }

        public IEnumerable<string> GetAssets(string assetsType)
        {
            if (Assets.TryGetValue(assetsType, out var assemblies))
            {
                return assemblies.Select(a => GetFullPath(a));
            }

            return Enumerable.Empty<string>();
        }
    }
}
