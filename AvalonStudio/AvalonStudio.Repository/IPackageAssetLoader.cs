using AvalonStudio.Extensibility.Plugin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Packages
{
    public interface IPackageAssetLoader : IExtension
    {
        Task LoadAssetsAsync(string package, string version, IEnumerable<string> files);
    }
}
