using System.Collections.Generic;

namespace AvalonStudio.Extensibility
{
    public interface IExtensionManifest
    {
        string Name { get; }
        string Version { get; }
        string Description { get; }
        string Icon { get; }

        IDictionary<string, IEnumerable<string>> Assets { get; }

        IDictionary<string, object> AdditionalData { get; set; }

        IEnumerable<string> GetMefComponents();
    }
}
