using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Packaging
{
    public class PackageManifest
    {
        public PackageManifest()
        {
            Properties = new Dictionary<string, object>();
        }

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public string Version { get; set; }

        public Dictionary<string, object> Properties { get; set; }

        public static PackageManifest Load(string file, string name)
        {
            var result = SerializedObject.Deserialize<PackageManifest>(file);

            result.Version = Path.GetFileName(Path.GetDirectoryName(file));
            result.Name = name;

            return result;
        }

        public void Save(string path)
        {
            SerializedObject.Serialize(path, this);
        }

        public string ResolvePackagePath(string url, bool appendExecutableExtension = true)
        {
            string result = "";

            var packageInfo = ParseUrl(url);

            var fullPackageId = (packageInfo.package + packageInfo.version).ToLower();

            var packageLocation = "";

            packageLocation = PackageManager.GetPackageDirectory(packageInfo.package, packageInfo.version).ToPlatformPath();

            result = (Path.Combine(packageLocation, packageInfo.location) + (appendExecutableExtension ? Platform.ExecutableExtension : "")).ToPlatformPath();

            return result;
        }

        public async Task<string> ResolvePackagePathAsync(string url, bool appendExecutableExtension = true, IConsole console = null)
        {
            string result = "";

            var packageInfo = ParseUrl(url);

            var fullPackageId = (packageInfo.package + packageInfo.version).ToLower();

            var packageLocation = "";


            if (await PackageManager.EnsurePackage(packageInfo.package, packageInfo.version, console) == PackageEnsureStatus.NotFound)
            {
                throw new Exception("Package not found.");
            }

            packageLocation = PackageManager.GetPackageDirectory(packageInfo.package, packageInfo.version).ToPlatformPath();

            result = (Path.Combine(packageLocation, packageInfo.location) + (appendExecutableExtension ? Platform.ExecutableExtension : "")).ToPlatformPath();

            return result;
        }

        public (string package, string version, string location) ParseUrl(string url)
        {
            var result = PackageManager.ParseUrl(url);

            if (result.package == null && result.version == null && result.location == null)
            {
                return (Name, Version, url);
            }
            else
            {
                return result;
            }
        }
    }
}
