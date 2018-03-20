using System.Collections.Generic;

namespace AvalonStudio.Extensibility
{
    public interface IExtensionManager
    {
        IEnumerable<IExtensionManifest> GetInstalledExtensions();
    }
}
