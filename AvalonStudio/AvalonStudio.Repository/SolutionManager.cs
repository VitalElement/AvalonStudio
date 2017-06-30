using AvalonStudio.Platforms;
using NuGet.PackageManagement;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Packages
{
    internal class SolutionManager : ISolutionManager
    {
        public string SolutionDirectory => Platform.RepoCatalogDirectory;

        public string DefaultNuGetProjectName { get; set; }

        public NuGetProject DefaultNuGetProject => throw new NotImplementedException();

        public bool IsSolutionOpen => true;

        public bool IsSolutionAvailable => true;

        public bool IsSolutionDPLEnabled => false;

        public INuGetProjectContext NuGetProjectContext { get; set; }

        public event EventHandler SolutionOpening;

        public event EventHandler SolutionOpened;

        public event EventHandler SolutionClosing;

        public event EventHandler SolutionClosed;

        public event EventHandler<NuGetEventArgs<string>> AfterNuGetCacheUpdated;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectAdded;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectRemoved;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectRenamed;

        public event EventHandler<NuGetProjectEventArgs> NuGetProjectUpdated;

        public event EventHandler<NuGetProjectEventArgs> AfterNuGetProjectRenamed;

        public event EventHandler<ActionsExecutedEventArgs> ActionsExecuted;

        public void EnsureSolutionIsLoaded()
        {
        }

        public NuGetProject GetNuGetProject(string nuGetProjectSafeName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NuGetProject> GetNuGetProjects()
        {
            using (var installedPackageCache = PackageManager.GetCache())
            {
                yield return new AvalonStudioExtensionsFolderProject(PackageManager.GetFramework(), installedPackageCache, Platform.ReposDirectory);
            }
        }

        public string GetNuGetProjectSafeName(NuGetProject nuGetProject)
        {
            throw new NotImplementedException();
        }

        public void OnActionsExecuted(IEnumerable<ResolvedAction> actions)
        {
            throw new NotImplementedException();
        }

        public void SaveProject(NuGetProject nuGetProject)
        {
        }

        public Task<NuGetProject> UpdateNuGetProjectToPackageRef(NuGetProject oldProject)
        {
            throw new NotImplementedException();
        }
    }

    public class DeleteOnRestartManager : IDeleteOnRestartManager
    {
        private List<string> packagesToDelete = new List<string>();

        public event EventHandler<PackagesMarkedForDeletionEventArgs> PackagesMarkedForDeletionFound;

        public void CheckAndRaisePackageDirectoriesMarkedForDeletion()
        {
            PackagesMarkedForDeletionFound?.Invoke(this, new PackagesMarkedForDeletionEventArgs(packagesToDelete));
        }

        public void DeleteMarkedPackageDirectories(INuGetProjectContext projectContext)
        {
            foreach (var package in packagesToDelete)
            {
                Directory.Delete(package, true);
            }
        }

        public IReadOnlyList<string> GetPackageDirectoriesMarkedForDeletion()
        {
            return packagesToDelete;
        }

        public void MarkPackageDirectoryForDeletion(PackageIdentity package, string packageDirectory, INuGetProjectContext projectContext)
        {
            if (Directory.Exists(packageDirectory))
            {
                packagesToDelete.Add(packageDirectory);
            }
        }
    }
}