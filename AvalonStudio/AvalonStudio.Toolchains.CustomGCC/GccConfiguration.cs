using AvalonStudio.Extensibility;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.CustomGCC
{
    public class GccToolchainDescription
    {
        public string Id { get; set; }

        public Version Version { get; set; }

        public string CC { get; set; }

        public string Cpp { get; set; }

        public string AR { get; set; }

        public string LD { get; set; }

        public string Size { get; set; }

        public string Gdb { get; set; }

        public async Task<GccConfiguration> ToConfigAsync(bool autoInstall = true)
        {
            var result = new GccConfiguration(this);

            if (autoInstall)
            {
                await result.ResolveAsync();
            }

            return result;
        }

        public static GccToolchainDescription Load(string file)
        {
            return SerializedObject.Deserialize<GccToolchainDescription>(file);
        }

        public void Save (string path)
        {
            SerializedObject.Serialize(path, this);
        }
    }

    public class GccConfigurationsManager
    {
        private static Dictionary<string, List<GccConfiguration>> s_registeredConfigurations = new Dictionary<string, List<GccConfiguration>>();

        public static void Register(GccConfiguration configuration)
        {
            if (!s_registeredConfigurations.ContainsKey(configuration.Id))
            {
                s_registeredConfigurations.Add(configuration.Id, new List<GccConfiguration>());
            }

            if (!s_registeredConfigurations[configuration.Id].Contains(configuration))
            {
                s_registeredConfigurations[configuration.Id].InsertSorted(configuration);
            }
        }

        public static async Task<IEnumerable<PackageMetaData>> GetRemotePackagesAsync()
        {
            var packages = (await PackageManager.ListPackagesAsync(100)).Where(p=>p.Tags.Contains("gccdescription"));
            
            return packages;
        }

        public static IEnumerable<string> Packages => s_registeredConfigurations.Keys;

        public static IEnumerable<GccConfiguration> GetConfigurations(string id)
        {
            if (s_registeredConfigurations.ContainsKey(id))
            {
                return s_registeredConfigurations[id];
            }
            else
            {
                return null;
            }
        }

        public static GccConfiguration GetConfiguration(string id, string version)
        {
            var parsedVersion = Version.Parse(version);

            if(parsedVersion.Revision == -1)
            {
                parsedVersion = new Version(parsedVersion.Major, parsedVersion.Minor, parsedVersion.Build, 0);
            }

            if (s_registeredConfigurations.ContainsKey(id))
            {
                return s_registeredConfigurations[id].FirstOrDefault(c=>c.Version == parsedVersion.ToString(4));
            }
            else
            {
                return null;
            }
        }
    }

    public class GccConfiguration : IComparable<GccConfiguration>
    {
        private string _cc;
        private string _cpp;
        private string _ar;
        private string _ld;
        private string _gdb;
        private string _size;
        private Version _version;
        private string _id;
        private GccToolchainDescription _description;

        private bool _isResolved;
        private Dictionary<string, string> _resolvedPackages = new Dictionary<string, string>();

        internal GccConfiguration(GccToolchainDescription description)
        {
            _description = description;
            _id = description.Id;
            _version = description.Version;
        }

        public async Task<bool> ResolveAsync()
        {
            if (!_isResolved)
            {
                _isResolved = true;

                try
                {
                    CC = await ResolvePackage(_description.CC);
                    Cpp = await ResolvePackage(_description.Cpp);
                    AR = await ResolvePackage(_description.AR);
                    LD = await ResolvePackage(_description.LD);
                    Size = await ResolvePackage(_description.Size);
                    Gdb = await ResolvePackage(_description.Gdb);
                }
                catch (Exception)
                {
                    _isResolved = false;
                }
            }

            return _isResolved;
        }

        private async Task<string> ResolvePackage(string url)
        {
            string result = "";

            var console = IoC.Get<IConsole>();

            var packageInfo = ParseUrl(url);

            var fullPackageId = (packageInfo.package + packageInfo.version).ToLower();

            var packageLocation = "";

            if (_resolvedPackages.ContainsKey(fullPackageId))
            {
                packageLocation = _resolvedPackages[fullPackageId];
            }
            else
            {
                if (await PackageManager.EnsurePackage(packageInfo.package, packageInfo.version, console) == PackageEnsureStatus.NotFound)
                {
                    throw new Exception("Package not found.");
                }
                
                packageLocation = PackageManager.GetPackageDirectory(packageInfo.package, packageInfo.version);

                _resolvedPackages.Add(fullPackageId, packageLocation);
            }

            result = (Path.Combine(packageLocation, packageInfo.location) + Platform.ExecutableExtension).ToPlatformPath();

            return result;
        }

        private (string package, string version, string location) ParseUrl(string url)
        {
            string location = "";
            string package = "";
            string version = "";

            if (url.Contains("?"))
            {
                var urlQueryOperatorIndex = url.IndexOf('?');
                location = url.Substring(0, urlQueryOperatorIndex);

                var parameters = url.Substring(urlQueryOperatorIndex + 1, url.Length - (urlQueryOperatorIndex + 1));

                var parameterParts = parameters.Split('&');

                foreach (var param in parameterParts)
                {
                    var valueKey = param.Split('=');

                    if (valueKey.Length == 2)
                    {
                        switch (valueKey[0])
                        {
                            case "Package":
                                package = valueKey[1];
                                break;

                            case "Version":
                                version = valueKey[1];
                                break;
                        }
                    }
                }

                return (package, version, location);
            }
            else
            {
                return ("", "", url);
            }
        }

        public int CompareTo(GccConfiguration other)
        {
            var idCompare = _id.CompareTo(other._id);
            
            if(idCompare == 0)
            {
                return _version.CompareTo(other._version);
            }
            else
            {
                return idCompare;
            }
        }

        public string Id => _id;

        public string Version => _version.ToString(4);

        public string CC
        {
            get { return _cc; }
            private set { _cc = value; }
        }

        public string Cpp
        {
            get { return _cpp; }
            private set { _cpp = value; }
        }

        public string AR
        {
            get { return _ar; }
            private set { _ar = value; }
        }

        public string LD
        {
            get { return _ld; }
            private set { _ld = value; }
        }

        public string Gdb
        {
            get { return _gdb; }
            set { _gdb = value; }
        }

        public string Size
        {
            get { return _size; }
            set { _size = value; }
        }
    }
}
