using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace AvalonStudio.Packages
{
    public class InstalledPackagesCache : IDisposable
    {
        private readonly List<CachedPackage> _installedPackages = new List<CachedPackage>(); // The packages installed during this session
        private readonly string _cacheFilePath;
        private readonly string _installedFilePath;
        private readonly List<CachedPackage> _cachedPackages = new List<CachedPackage>();

        public class EmptyDisposable : IDisposable
        {
            public static EmptyDisposable Instance = new EmptyDisposable();

            public void Dispose()
            {
                // Do nothing
            }
        }

        private CachedPackageEntry _currentlyInstallingPackage = null;

        public InstalledPackagesCache(string cachePath, string installedPath, bool updatePackages)
        {
            _installedFilePath = installedPath ?? throw new ArgumentException(nameof(installedPath));
            _cacheFilePath = cachePath ?? throw new ArgumentNullException(nameof(cachePath));

            _cachedPackages = ReadCacheFile(cachePath, updatePackages).ToList();
            _installedPackages = ReadCacheFile(installedPath, updatePackages).ToList();
        }

        private static CachedPackage[] ReadCacheFile(string fullPath, bool updatePackages)
        {
            if (!updatePackages && File.Exists(fullPath))
            {
                try
                {
                    XDocument packagesDocument = XDocument.Load(fullPath);
                    if (packagesDocument.Root == null)
                    {
                        Trace.TraceWarning("No root element in packages file");
                    }
                    else if (packagesDocument.Root.Name != CachedPackage.PackagesElementName)
                    {
                        Trace.TraceWarning($@"Packages file root element should be named ""{CachedPackage.PackagesElementName}"" but is actually named ""{packagesDocument.Root.Name}""");
                    }
                    else
                    {
                        return packagesDocument.Root.Elements(CachedPackage.PackageElementName).Select(x => new CachedPackage(x)).ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceWarning($"Error while reading packages file: {ex.Message}");
                }
            }
            return Array.Empty<CachedPackage>();
        }

        /// <summary>
        /// Gets all installed packages from this session and their dependencies.
        /// </summary>
        public IEnumerable<PackageIdentity> GetInstalledPackagesAndDependencies() =>
            _installedPackages.Select(x => x.PackageReference.PackageIdentity)
                .Concat(_installedPackages.SelectMany(x => x.Dependencies.Select(dep => dep.PackageIdentity)));

        /// <summary>
        /// Verifies that a package has been previously installed as well as
        /// currently existing locally with all dependencies, and if so,
        /// adds it back to the outgoing cache file along with all it's
        /// previously calculated dependencies.
        /// </summary>
        public bool VerifyPackage(PackageIdentity packageIdentity, NuGetPackageManager packageManager)
        {
            CachedPackage cached = _cachedPackages.FirstOrDefault(x =>
                x.PackageReference.PackageIdentity.Id.Equals(packageIdentity.Id, StringComparison.OrdinalIgnoreCase)
                && x.PackageReference.PackageIdentity.Version.Equals(packageIdentity.Version));
            if (cached != null && cached.VerifyPackage(packageManager))
            {
                _installedPackages.Add(cached);
                return true;
            }
            return false;
        }

        public IDisposable AddPackage(PackageIdentity identity, NuGetFramework targetFramework)
        {
            if (_currentlyInstallingPackage != null && _currentlyInstallingPackage.CurrentlyInstalling)
            {
                // We're currently installing a package, so add the dependency to that one
                // Make sure this isn't the actual top-level package
                if (!_currentlyInstallingPackage.CachedPackage.PackageReference.PackageIdentity.Equals(identity)
                    || !_currentlyInstallingPackage.CachedPackage.PackageReference.TargetFramework.Equals(targetFramework))
                {
                    _currentlyInstallingPackage.CachedPackage.AddDependency(new CachedPackage(identity, targetFramework));
                }
                return EmptyDisposable.Instance;
            }

            // This is a new top-level installation, so add to the root
            CachedPackage cachedPackage = new CachedPackage(identity, targetFramework);
            _installedPackages.Add(cachedPackage);
            _currentlyInstallingPackage = new CachedPackageEntry(cachedPackage);
            return _currentlyInstallingPackage;
        }

        public IDisposable RemovePackage(PackageIdentity identity, NuGetFramework targetFramework)
        {
            var current = _installedPackages.FirstOrDefault(p => p.PackageReference.PackageIdentity == identity);

            if (current != null)
            {
                _installedPackages.Remove(current);
            }

            current = _cachedPackages.FirstOrDefault(p => p.PackageReference.PackageIdentity == identity);

            if (current != null)
            {
                _cachedPackages.Remove(current);
            }

            return EmptyDisposable.Instance;
        }

        public void Dispose()
        {
            new XDocument(
                new XElement(CachedPackage.PackagesElementName,
                    _installedPackages.Select(x => (object)x.Element).ToArray())).Save(_cacheFilePath);

            new XDocument(
                new XElement(CachedPackage.PackagesElementName,
                    _installedPackages.Select(x => (object)x.Element).ToArray())).Save(_installedFilePath);
        }

        private class CachedPackageEntry : IDisposable
        {
            public CachedPackage CachedPackage { get; }

            public bool CurrentlyInstalling { get; private set; }

            public CachedPackageEntry(CachedPackage cachedPackage)
            {
                CachedPackage = cachedPackage;
                CurrentlyInstalling = true;
            }

            public void Dispose()
            {
                CurrentlyInstalling = false;
            }
        }
    }
}