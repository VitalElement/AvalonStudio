using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Packages
{
    public interface IPackageAssetLoader
    {
        Task LoadAssetsAsync(string package, string version, IEnumerable<string> files);
    }
}
