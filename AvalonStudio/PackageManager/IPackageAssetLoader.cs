using AvalonStudio.Extensibility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Packages
{
    public interface IPackageAssetLoader : IActivatableExtension
    {
        Task LoadAssetsAsync(IEnumerable<string> files);
    }
}
