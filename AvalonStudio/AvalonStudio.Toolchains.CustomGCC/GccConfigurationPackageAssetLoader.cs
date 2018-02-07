using AvalonStudio.Packages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.CustomGCC
{
    class GccConfigurationPackageAssetLoader : IPackageAssetLoader
    {
        public void Activation()
        {
            PackageManager.RegisterAssetLoader(this);
        }

        public void BeforeActivation()
        {
        }

        public async Task LoadAssetsAsync(string package, string version, IEnumerable<string> files)
        {
            foreach(var file in files.Where(f=> Path.GetExtension(f) == ".gccdescription"))
            {
                var description = GccToolchainDescription.Load(file);

                var config = await description.ToConfigAsync(false);

                GccConfigurationsManager.Register(config);
            }
        }
    }
}
