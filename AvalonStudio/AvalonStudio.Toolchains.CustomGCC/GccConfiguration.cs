using AvalonStudio.Extensibility;
using AvalonStudio.Packaging;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.CustomGCC
{
    public class GccConfiguration : IComparable<GccConfiguration>
    {
        private List<string> _systemIncludePaths;
        private List<string> _systemLibraryPaths;

        private PackageManifest _description;

        private bool _isResolved;
        private Dictionary<string, string> _resolvedPackages = new Dictionary<string, string>();

        internal GccConfiguration(PackageManifest description)
        {
            _description = description;
        }

        public static GccConfiguration FromManifest(PackageManifest manifest)
        {
            return new GccConfiguration(manifest);
        }

        public async Task<bool> ResolveAsync()
        {
            if (!_isResolved)
            {
                _isResolved = true;

                _systemIncludePaths = new List<string>();
                _systemLibraryPaths = new List<string>();

                var console = IoC.Get<IConsole>();

                try
                {
                    if (_description.Properties.TryGetValue("Gcc.CC", out var cc))
                    {
                        CC = await _description.ResolvePackagePath(cc as string, console: console);
                    }

                    if (_description.Properties.TryGetValue("Gcc.CXX", out var cxx))
                    {
                        Cpp = await _description.ResolvePackagePath(cxx as string, console: console);
                    }

                    if (_description.Properties.TryGetValue("Gcc.AR", out var ar))
                    {
                        AR = await _description.ResolvePackagePath(ar as string, console: console);
                    }

                    if (_description.Properties.TryGetValue("Gcc.LD", out var ld))
                    {
                        LD = await _description.ResolvePackagePath(ld as string, console: console);
                    }

                    if (_description.Properties.TryGetValue("Gcc.SIZE", out var size))
                    {
                        Size = await _description.ResolvePackagePath(size as string, console: console);
                    }

                    if (_description.Properties.TryGetValue("Gcc.GDB", out var gdb))
                    {
                        Gdb = await _description.ResolvePackagePath(gdb as string, console: console);
                    }

                    if(_description.Properties.TryGetValue("Gcc.SystemIncludePaths", out var systemIncludePaths))
                    { 
                        foreach (var unresolvedPath in (systemIncludePaths as JArray).ToList().Select(x=>x.ToString()))
                        {
                            _systemIncludePaths.Add(await _description.ResolvePackagePath(unresolvedPath, false, console: console));
                        }
                    }

                    if (_description.Properties.TryGetValue("Gcc.SystemLibraryPaths", out var systemLibraryPaths))
                    {
                        foreach (var unresolvedPath in (systemLibraryPaths as JArray).ToList().Select(x => x.ToString()))
                        {
                            _systemLibraryPaths.Add(await _description.ResolvePackagePath(unresolvedPath, false, console: console));
                        }
                    }
                }
                catch (Exception)
                {
                    _isResolved = false;
                }
            }

            return _isResolved;
        }

        public int CompareTo(GccConfiguration other)
        {
            return 0;
        }

        public List<string> SystemIncludePaths
        {
            get => _systemIncludePaths;
            private set => _systemIncludePaths = value;
        }

        public List<string> SystemLibraryPaths
        {
            get => _systemLibraryPaths;
        }

        public string CC { get; private set; }

        public string Cpp { get; private set; }

        public string AR { get; private set; }

        public string LD { get; private set; }

        public string Gdb { get; set; }

        public string Size { get; set; }
    }
}
