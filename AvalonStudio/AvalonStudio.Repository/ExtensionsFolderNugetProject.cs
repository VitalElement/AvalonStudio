using NuGet;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Packages
{
    // This primarily exists to intercept package installations and store their paths
    internal class AvalonStudioExtensionsFolderProject : FolderNuGetProject
    {
        private readonly IFileSystem _fileSystem;

        private readonly NuGetFramework _currentFramework;
        private readonly InstalledPackagesCache _installedPackages;

        public AvalonStudioExtensionsFolderProject(IFileSystem fileSystem, NuGetFramework currentFramework, InstalledPackagesCache installedPackages, string root)
            : base(root)
        {
            _fileSystem = fileSystem;
            _currentFramework = currentFramework;
            _installedPackages = installedPackages;
        }

        // This gets called for every package install, including dependencies, and is our only chance to handle dependency PackageIdentity instances
        public override Task<bool> InstallPackageAsync(PackageIdentity packageIdentity, DownloadResourceResult downloadResourceResult,
            INuGetProjectContext nugetProjectContext, CancellationToken token)
        {
            _installedPackages.AddPackage(packageIdentity, _currentFramework);
            Trace.TraceInformation($"Installing package or dependency {packageIdentity.Id} {(packageIdentity.HasVersion ? packageIdentity.Version.ToNormalizedString() : string.Empty)}");
            return base.InstallPackageAsync(packageIdentity, downloadResourceResult, nugetProjectContext, token);
        }

        public override Task<bool> UninstallPackageAsync(PackageIdentity packageIdentity, INuGetProjectContext nugetProjectContext, CancellationToken token)
        {
            _installedPackages.RemovePackage(packageIdentity, _currentFramework);
            return base.UninstallPackageAsync(packageIdentity, nugetProjectContext, token);
        }

        private static FrameworkSpecificGroup GetMostCompatibleGroup(NuGetFramework projectTargetFramework,
            IEnumerable<FrameworkSpecificGroup> itemGroups)
        {
            var reducer = new FrameworkReducer();
            var mostCompatibleFramework
                = reducer.GetNearest(projectTargetFramework, itemGroups.Select(i => i.TargetFramework));
            if (mostCompatibleFramework != null)
            {
                var mostCompatibleGroup
                    = itemGroups.FirstOrDefault(i => i.TargetFramework.Equals(mostCompatibleFramework));

                if (IsValid(mostCompatibleGroup))
                {
                    return mostCompatibleGroup;
                }
            }

            return null;
        }

        public override async Task<IEnumerable<NuGet.Packaging.PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
        {
            return await Task.Run(() => _installedPackages.GetInstalledPackagesAndDependencies().Select(pi => new NuGet.Packaging.PackageReference(pi, new NuGetFramework("AvalonStudio1.0"))));
        }

        private static bool IsValid(FrameworkSpecificGroup frameworkSpecificGroup)
        {
            if (frameworkSpecificGroup != null)
            {
                return frameworkSpecificGroup.HasEmptyFolder
                     || frameworkSpecificGroup.Items.Any()
                     || !frameworkSpecificGroup.TargetFramework.Equals(NuGetFramework.AnyFramework);
            }

            return false;
        }
    }
}