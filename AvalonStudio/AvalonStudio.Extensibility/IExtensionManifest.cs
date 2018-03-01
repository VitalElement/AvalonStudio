using System;
using System.Collections.Generic;

namespace AvalonStudio.Extensibility
{
    public interface IExtensionManifest
    {
        string Name { get; }
        Version Version { get; }
        string Description { get; }
        string Icon { get; }

        IReadOnlyDictionary<string, IEnumerable<string>> Assets { get; }

        IReadOnlyDictionary<string, object> AdditionalData { get; }

        IEnumerable<string> GetAssets(string assetsType);
    }
}
