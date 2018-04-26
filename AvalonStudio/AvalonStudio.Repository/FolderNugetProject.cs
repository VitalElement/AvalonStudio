// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.PackageManagement;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Packaging.PackageExtraction;
using NuGet.Packaging.Signing;
using NuGet.ProjectManagement;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace AvalonStudio.Repository
{
    /// <summary>
    /// This class represents a NuGetProject based on a folder such as packages folder on a VisualStudio solution
    /// </summary>
    public class FolderNuGetProject : NuGetProject
    {
        /// <summary>
        /// Gets the folder project's root path.
        /// </summary>
        public string Root { get; set; }

        private PackagePathResolver PackagePathResolver { get; set; }

        private readonly NuGetFramework _framework;

        /// <summary>
        /// Initializes a new <see cref="FolderNuGetProject" /> class.
        /// </summary>
        /// <param name="root">The folder project root path.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="root" /> is <c>null</c>.</exception>
        public FolderNuGetProject(string root)
            : this(root, new PackagePathResolver(root))
        {
        }

        /// <summary>
        /// Initializes a new <see cref="FolderNuGetProject" /> class.
        /// </summary>
        /// <param name="root">The folder project root path.</param>
        /// <param name="packagePathResolver">A package path resolver.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="root" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packagePathResolver" />
        /// is <c>null</c>.</exception>
        public FolderNuGetProject(string root, PackagePathResolver packagePathResolver)
            : this(root, packagePathResolver, NuGetFramework.AnyFramework)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="FolderNuGetProject" /> class.
        /// </summary>
        /// <param name="root">The folder project root path.</param>
        /// <param name="packagePathResolver">A package path resolver.</param>
        /// <param name="targetFramework">Project target framework.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="root" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packagePathResolver" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="targetFramework" /> is <c>null</c>.</exception>
        public FolderNuGetProject(string root, PackagePathResolver packagePathResolver, NuGetFramework targetFramework)
        {
            if (targetFramework == null)
            {
                throw new ArgumentNullException(nameof(targetFramework));
            }

            Root = root ?? throw new ArgumentNullException(nameof(root));
            PackagePathResolver = packagePathResolver ?? throw new ArgumentNullException(nameof(packagePathResolver));

            InternalMetadata.Add(NuGetProjectMetadataKeys.Name, root);
            InternalMetadata.Add(NuGetProjectMetadataKeys.TargetFramework, targetFramework);
            _framework = targetFramework;
        }

        /// <summary>
        /// Asynchronously gets installed packages.
        /// </summary>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns an
        /// <see cref="IEnumerable{PackageReference}" />.</returns>
        public override Task<IEnumerable<PackageReference>> GetInstalledPackagesAsync(CancellationToken token)
        {
            return Task.FromResult(Enumerable.Empty<PackageReference>());
        }

        /// <summary>
        /// Asynchronously installs a package.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="downloadResourceResult">A download resource result.</param>
        /// <param name="nuGetProjectContext">A NuGet project context.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="bool" />
        /// indication successfulness of the operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="downloadResourceResult" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nuGetProjectContext" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">Thrown if the package stream for
        /// <paramref name="downloadResourceResult" /> is not seekable.</exception>
        public override Task<bool> InstallPackageAsync(
            PackageIdentity packageIdentity,
            DownloadResourceResult downloadResourceResult,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (downloadResourceResult == null)
            {
                throw new ArgumentNullException(nameof(downloadResourceResult));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            if (downloadResourceResult.Status == DownloadResourceResultStatus.Available &&
                !downloadResourceResult.PackageStream.CanSeek)
            {
                throw new ArgumentException("package steam should be searchable.", nameof(downloadResourceResult));
            }

            var packageDirectory = PackagePathResolver.GetInstallPath(packageIdentity);

            return ConcurrencyUtilities.ExecuteWithFileLockedAsync(
                packageDirectory,
                action: async cancellationToken =>
                {
                    // 1. Set a default package extraction context, if necessary.
                    var packageExtractionContext = nuGetProjectContext.PackageExtractionContext;

                    if (packageExtractionContext == null)
                    {
                        var signedPackageVerifier = !downloadResourceResult.SignatureVerified ? new PackageSignatureVerifier(
                            SignatureVerificationProviderFactory.GetSignatureVerificationProviders(),
                            SignedPackageVerifierSettings.Default) : null;

                        packageExtractionContext = new PackageExtractionContext(
                            PackageSaveMode.Defaultv2,
                            PackageExtractionBehavior.XmlDocFileSaveMode,
                            new LoggerAdapter(nuGetProjectContext),
                            signedPackageVerifier);
                    }

                    // 2. Check if the Package already exists at root, if so, return false
                    if (PackageExists(packageIdentity, packageExtractionContext.PackageSaveMode))
                    {
                        nuGetProjectContext.Log(MessageLevel.Info, "Package already exists in folder", packageIdentity, Root);
                        return false;
                    }

                    nuGetProjectContext.Log(MessageLevel.Info, "Adding package to folder.", packageIdentity, Path.GetFullPath(Root));

                    // 3. Call PackageExtractor to extract the package into the root directory of this FileSystemNuGetProject
                    if (downloadResourceResult.Status == DownloadResourceResultStatus.Available)
                    {
                        downloadResourceResult.PackageStream.Seek(0, SeekOrigin.Begin);
                    }
                    var addedPackageFilesList = new List<string>();

                    if (downloadResourceResult.PackageReader != null)
                    {
                        if (downloadResourceResult.Status == DownloadResourceResultStatus.AvailableWithoutStream)
                        {
                            addedPackageFilesList.AddRange(
                                await PackageExtractor.ExtractPackageAsync(
                                    downloadResourceResult.PackageReader,
                                    PackagePathResolver,
                                    packageExtractionContext,
                                    cancellationToken));
                        }
                        else
                        {
                            addedPackageFilesList.AddRange(
                                await PackageExtractor.ExtractPackageAsync(
                                    downloadResourceResult.PackageReader,
                                    downloadResourceResult.PackageStream,
                                    PackagePathResolver,
                                    packageExtractionContext,
                                    cancellationToken));
                        }
                    }
                    else
                    {
                        addedPackageFilesList.AddRange(
                            await PackageExtractor.ExtractPackageAsync(
                                downloadResourceResult.PackageStream,
                                PackagePathResolver,
                                packageExtractionContext,
                                cancellationToken));
                    }

                    var packageSaveMode = GetPackageSaveMode(nuGetProjectContext);
                    if (packageSaveMode.HasFlag(PackageSaveMode.Nupkg))
                    {
                        var packageFilePath = GetInstalledPackageFilePath(packageIdentity);
                        if (File.Exists(packageFilePath))
                        {
                            addedPackageFilesList.Add(packageFilePath);
                        }
                    }

                    // Pend all the package files including the nupkg file
                    FileSystemUtility.PendAddFiles(addedPackageFilesList, Root, nuGetProjectContext);

                    nuGetProjectContext.Log(MessageLevel.Info, "Added Package to Folder.", packageIdentity, Path.GetFullPath(Root));

                    // Extra logging with source for verbosity detailed
                    // Used by external tool CoreXT to track package provenance
                    if (!string.IsNullOrEmpty(downloadResourceResult.PackageSource))
                    {
                        nuGetProjectContext.Log(MessageLevel.Debug, "Added package to Folder from Source.", packageIdentity, Path.GetFullPath(Root), downloadResourceResult.PackageSource);
                    }

                    return true;
                },
                token: token);
        }

        /// <summary>
        /// Asynchronously uninstalls a package.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="nuGetProjectContext">A NuGet project context.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="bool" />
        /// indication successfulness of the operation.</returns>
        public override Task<bool> UninstallPackageAsync(
            PackageIdentity packageIdentity,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            // Do nothing. Return true
            return Task.FromResult(true);
        }

        /// <summary>
        /// Determines if a package is installed based on the presence of a .nupkg file.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <returns>A flag indicating whether or not the package is installed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        public bool PackageExists(PackageIdentity packageIdentity)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            return PackageExists(packageIdentity, PackageSaveMode.Nupkg);
        }

        /// <summary>
        /// Determines if a package is installed based on the provided package save mode.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="packageSaveMode">A package save mode.</param>
        /// <returns>A flag indicating whether or not the package is installed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        public bool PackageExists(PackageIdentity packageIdentity, PackageSaveMode packageSaveMode)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            var nupkgPath = GetInstalledPackageFilePath(packageIdentity);
            var nuspecPath = GetInstalledManifestFilePath(packageIdentity);

            var packageExists = !string.IsNullOrEmpty(nupkgPath);
            var manifestExists = !string.IsNullOrEmpty(nuspecPath);

            // When using -ExcludeVersion check that the actual package version matches.
            if (!PackagePathResolver.UseSideBySidePaths)
            {
                if (packageExists)
                {
                    using (var reader = new PackageArchiveReader(nupkgPath))
                    {
                        packageExists = packageIdentity.Equals(reader.NuspecReader.GetIdentity());
                    }
                }

                if (manifestExists)
                {
                    var reader = new NuspecReader(nuspecPath);
                    packageExists = packageIdentity.Equals(reader.GetIdentity());
                }
            }

            if (!packageExists)
            {
                packageExists |= !string.IsNullOrEmpty(GetPackageDownloadMarkerFilePath(packageIdentity));
            }

            // A package must have either a nupkg or a nuspec to be valid
            var result = packageExists || manifestExists;

            // Verify nupkg present if specified
            if ((packageSaveMode & PackageSaveMode.Nupkg) == PackageSaveMode.Nupkg)
            {
                result &= packageExists;
            }

            // Verify nuspec present if specified
            if ((packageSaveMode & PackageSaveMode.Nuspec) == PackageSaveMode.Nuspec)
            {
                result &= manifestExists;
            }

            return result;
        }

        /// <summary>
        /// Determines if a manifest is installed.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <returns>A flag indicating whether or not the package is installed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        public bool ManifestExists(PackageIdentity packageIdentity)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            var path = GetInstalledManifestFilePath(packageIdentity);

            var exists = !string.IsNullOrEmpty(path);

            if (exists && !PackagePathResolver.UseSideBySidePaths)
            {
                var reader = new NuspecReader(path);
                exists = packageIdentity.Equals(reader.GetIdentity());
            }

            return exists;
        }

        /// <summary>
        /// Determines if a manifest is installed.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <returns>A flag indicating whether or not the package is installed.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        public bool PackageAndManifestExists(PackageIdentity packageIdentity)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            return !string.IsNullOrEmpty(GetInstalledPackageFilePath(packageIdentity)) && !string.IsNullOrEmpty(GetInstalledManifestFilePath(packageIdentity));
        }

        /// <summary>
        /// Asynchronously copies satellite files.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="nuGetProjectContext">A NuGet project context.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="bool" />
        /// indication successfulness of the operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nuGetProjectContext" />
        /// is <c>null</c>.</exception>
        /// <exception cref="OperationCanceledException">Thrown if <paramref name="token" />
        /// is cancelled.</exception>
        public async Task<bool> CopySatelliteFilesAsync(
            PackageIdentity packageIdentity,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            token.ThrowIfCancellationRequested();

            var packageExtractionContext = nuGetProjectContext.PackageExtractionContext;
            if (packageExtractionContext == null)
            {
                var signedPackageVerifier = new PackageSignatureVerifier(
                            SignatureVerificationProviderFactory.GetSignatureVerificationProviders(),
                            SignedPackageVerifierSettings.Default);

                packageExtractionContext = new PackageExtractionContext(
                    PackageSaveMode.Defaultv2,
                    PackageExtractionBehavior.XmlDocFileSaveMode,
                    new LoggerAdapter(nuGetProjectContext),
                    signedPackageVerifier);
            }

            var copiedSatelliteFiles = await PackageExtractor.CopySatelliteFilesAsync(
                packageIdentity,
                PackagePathResolver,
                GetPackageSaveMode(nuGetProjectContext),
                packageExtractionContext,
                token);

            FileSystemUtility.PendAddFiles(copiedSatelliteFiles, Root, nuGetProjectContext);

            return copiedSatelliteFiles.Any();
        }

        /// <summary>
        /// Gets the package .nupkg file path if it exists; otherwise, <c>null</c>.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <returns>The package .nupkg file path if it exists; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        public string GetInstalledPackageFilePath(PackageIdentity packageIdentity)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            // Check the expected location before searching all directories
            var packageDirectory = PackagePathResolver.GetInstallPath(packageIdentity);
            var packageName = PackagePathResolver.GetPackageFileName(packageIdentity);

            var installPath = Path.GetFullPath(Path.Combine(packageDirectory, packageName));

            // Keep the previous optimization of just going by the existance of the file if we find it.
            if (File.Exists(installPath))
            {
                return installPath;
            }

            // If the file was not found check for non-normalized paths and verify the id/version
            LocalPackageInfo package = null;

            if (PackagePathResolver.UseSideBySidePaths)
            {
                // Search for a folder with the id and version
                package = LocalFolderUtility.GetPackagesConfigFolderPackage(
                    Root,
                    packageIdentity,
                    NullLogger.Instance);
            }
            else
            {
                // Search for just the id
                package = LocalFolderUtility.GetPackageV2(
                    Root,
                    packageIdentity,
                    NullLogger.Instance);
            }

            if (package != null && packageIdentity.Equals(package.Identity))
            {
                return package.Path;
            }

            // Default to empty
            return string.Empty;
        }

        /// <summary>
        /// Gets the package .nuspec file path if it exists; otherwise, <c>null</c>.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <returns>The package .nuspec file path if it exists; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        public string GetInstalledManifestFilePath(PackageIdentity packageIdentity)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            // Check the expected location before searching all directories
            var packageDirectory = PackagePathResolver.GetInstallPath(packageIdentity);
            var manifestName = PackagePathResolver.GetManifestFileName(packageIdentity);

            var installPath = Path.GetFullPath(Path.Combine(packageDirectory, manifestName));

            // Keep the previous optimization of just going by the existance of the file if we find it.
            if (File.Exists(installPath))
            {
                return installPath;
            }

            // Don't look in non-normalized paths for nuspec
            return string.Empty;
        }

        /// <summary>
        /// Gets the package download marker file path if it exists; otherwise, <c>null</c>.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <returns>The package download marker file path if it exists; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        public string GetPackageDownloadMarkerFilePath(PackageIdentity packageIdentity)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            var packageDirectory = PackagePathResolver.GetInstallPath(packageIdentity);
            var fileName = PackagePathResolver.GetPackageDownloadMarkerFileName(packageIdentity);

            var filePath = Path.GetFullPath(Path.Combine(packageDirectory, fileName));

            // Keep the previous optimization of just going by the existance of the file if we find it.
            if (File.Exists(filePath))
            {
                return filePath;
            }

            return null;
        }

        /// <summary>
        /// Gets the package directory path if the package exists; otherwise, <c>null</c>.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <returns>The package directory path if the package exists; otherwise, <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        public string GetInstalledPath(PackageIdentity packageIdentity)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            var installFilePath = GetInstalledPackageFilePath(packageIdentity);

            if (!string.IsNullOrEmpty(installFilePath))
            {
                return Path.GetDirectoryName(installFilePath);
            }

            // Default to empty
            return string.Empty;
        }

        /// <summary>
        /// Asynchronously deletes a package.
        /// </summary>
        /// <param name="packageIdentity">A package identity.</param>
        /// <param name="nuGetProjectContext">A NuGet project context.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result (<see cref="Task{TResult}.Result" />) returns a <see cref="bool" />
        /// indication successfulness of the operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="packageIdentity" />
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="nuGetProjectContext" />
        /// is <c>null</c>.</exception>
        public async Task<bool> DeletePackage(PackageIdentity packageIdentity,
            INuGetProjectContext nuGetProjectContext,
            CancellationToken token)
        {
            if (packageIdentity == null)
            {
                throw new ArgumentNullException(nameof(packageIdentity));
            }

            if (nuGetProjectContext == null)
            {
                throw new ArgumentNullException(nameof(nuGetProjectContext));
            }

            var packageFilePath = GetInstalledPackageFilePath(packageIdentity);
            if (File.Exists(packageFilePath))
            {
                var packageDirectoryPath = Path.GetDirectoryName(packageFilePath);
                using (var packageReader = new PackageArchiveReader(packageFilePath))
                {
                    var installedSatelliteFilesPair = await PackageHelper.GetInstalledSatelliteFilesAsync(
                        packageReader,
                        PackagePathResolver,
                        GetPackageSaveMode(nuGetProjectContext),
                        token);
                    var runtimePackageDirectory = installedSatelliteFilesPair.Item1;
                    var installedSatelliteFiles = installedSatelliteFilesPair.Item2;
                    if (!string.IsNullOrEmpty(runtimePackageDirectory))
                    {
                        try
                        {
                            // Delete all the package files now
                            FileSystemUtility.DeleteFiles(installedSatelliteFiles, runtimePackageDirectory, nuGetProjectContext);
                        }
                        catch (Exception ex)
                        {
                            nuGetProjectContext.Log(MessageLevel.Warning, ex.Message);
                            // Catch all exception with delete so that the package file is always deleted
                        }
                    }

                    // Get all the package files before deleting the package file
                    var installedPackageFiles = await PackageHelper.GetInstalledPackageFilesAsync(
                        packageReader,
                        packageIdentity,
                        PackagePathResolver,
                        GetPackageSaveMode(nuGetProjectContext),
                        token);

                    try
                    {
                        // Delete all the package files now
                        FileSystemUtility.DeleteFiles(installedPackageFiles, packageDirectoryPath, nuGetProjectContext);
                    }
                    catch (Exception ex)
                    {
                        nuGetProjectContext.Log(MessageLevel.Warning, ex.Message);
                        // Catch all exception with delete so that the package file is always deleted
                    }
                }

                // Delete the package file
                FileSystemUtility.DeleteFile(packageFilePath, nuGetProjectContext);

                // Delete the package directory if any
                FileSystemUtility.DeleteDirectorySafe(packageDirectoryPath, recursive: true, nuGetProjectContext: nuGetProjectContext);

                // If this is the last package delete the package directory
                // If this is the last package delete the package directory
                if (!FileSystemUtility.GetFiles(Root, string.Empty, "*.*").Any()
                    && !FileSystemUtility.GetDirectories(Root, string.Empty).Any())
                {
                    FileSystemUtility.DeleteDirectorySafe(Root, recursive: false, nuGetProjectContext: nuGetProjectContext);
                }
            }

            return true;
        }

        private PackageSaveMode GetPackageSaveMode(INuGetProjectContext nuGetProjectContext)
        {
            return nuGetProjectContext.PackageExtractionContext?.PackageSaveMode ?? PackageSaveMode.Defaultv2;
        }
    }
}