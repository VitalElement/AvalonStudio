using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Packages
{
    public class PackageMetaData
    {
        private IPackageSearchMetadata _metaData;

        public PackageMetaData(IPackageSearchMetadata metaData)
        {
            _metaData = metaData;
        }

        public PackageIdentity Identity => _metaData.Identity;
        public string Title => _metaData.Identity.Id;
        public string Tags => _metaData.Tags;

        public async Task<IEnumerable<string>> GetVersionsAsync()
        {
            return (await _metaData.GetAllVersionsAsync()).Select(v => v.Version.Version.ToString(4));
        }

        public override string ToString()
        {
            return Title;
        }
    }

    public static class IPackageSearchMetadataExtensions
    {
        public static async Task<IEnumerable<VersionInfo>> GetAllVersionsAsync(this IPackageSearchMetadata metaData)
        {
            var versionData = await metaData.GetVersionsAsync();

            var baseVersion = new List<VersionInfo> { new VersionInfo(metaData.Identity.Version) };

            return versionData.Concat(baseVersion).OrderByDescending(v => v.Version);
        }
    }
}