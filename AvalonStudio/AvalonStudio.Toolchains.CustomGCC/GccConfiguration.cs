using AvalonStudio.Extensibility;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.CustomGCC
{
    public class GccToolchainDescription
    {
        public string CCUrl { get; set; }

        public string CppUrl { get; set; }

        public string ARUrl { get; set; }

        public string LDUrl { get; set; }

        public string SizeUrl { get; set; }

        public string GdbUrl { get; set; }

        public async Task<GccConfiguration> ToConfigAsync()
        {
            var result = new GccConfiguration();

            await result.Resolve(this);

            return result;
        }
    }

    public class GccConfiguration
    {
        private string _cc;
        private string _cpp;
        private string _ar;
        private string _ld;
        private string _gdb;
        private string _size;

        private bool _isResolved;
        private Dictionary<string, string> _resolvedPackages = new Dictionary<string, string>();

        internal async Task Resolve(GccToolchainDescription tc)
        {
            if (!_isResolved)
            {
                _isResolved = true;

                CC = await ResolvePackage(tc.CCUrl);
                Cpp = await ResolvePackage(tc.CppUrl);
                AR = await ResolvePackage(tc.ARUrl);
                LD = await ResolvePackage(tc.LDUrl);
                Size = await ResolvePackage(tc.SizeUrl);
                Gdb = await ResolvePackage(tc.GdbUrl);
            }
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
                if (await PackageManager.EnsurePackage(packageInfo.package, packageInfo.version, console))
                {
                    packageLocation = PackageManager.GetPackageDirectory(packageInfo.package, packageInfo.version);

                    _resolvedPackages.Add(fullPackageId, packageLocation);
                }
                else
                {
                    throw new Exception($"Unable to install or location package: {packageInfo.package}, {packageInfo.version}");
                }
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
