using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using ICSharpCode.SharpZipLib.Tar;
using ManagedLzma.SevenZip;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Mono.Unix;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Packaging
{
    public enum PackagePlatform
    {
        Unknown,
        WinX64,
        Osx,
        LinuxX64,
        Any
    }

    public enum PackageEnsureStatus
    {
        Unknown,
        NotFound,
        Found,
        Installed
    }

    public class PackageIdentifier
    {
        public string Name { get; set; }
        public Version Version { get; set; }
    }
    public class Package
    {
        public static string PackagePlatformName(PackagePlatform platform)
        {
            switch (platform)
            {
                case PackagePlatform.WinX64: return "win-x64";
                case PackagePlatform.Osx: return "osx-x64";
                case PackagePlatform.LinuxX64: return "linux-x64";
                case PackagePlatform.Any: return "any";
                default: return "";
            }
        }

        public static PackagePlatform GetPackagePlatformFromString(string platform)
        {
            switch (platform)
            {
                case "win-x64": return PackagePlatform.WinX64;
                case "linux-x64": return PackagePlatform.LinuxX64;
                case "osx-x64": return PackagePlatform.Osx;
                case "any": return PackagePlatform.Any;

                default: return PackagePlatform.Unknown;
            }
        }

        public Version Version { get; internal set; }

        public string Name { get; internal set; }

        public string BlobIdentity { get; internal set; }

        public PackagePlatform Platform { get; internal set; }

        public DateTime Published { get; internal set; }

        public Uri Uri { get; internal set; }

        public long Size { get; internal set; }
    }

    public class PackageManager
    {
        private const string sharedAccessString = "BlobEndpoint=https://avalonstudiotoolchains.blob.core.windows.net/;QueueEndpoint=https://avalonstudiotoolchains.queue.core.windows.net/;FileEndpoint=https://avalonstudiotoolchains.file.core.windows.net/;TableEndpoint=https://avalonstudiotoolchains.table.core.windows.net/;SharedAccessSignature=sv=2018-03-28&ss=b&srt=sco&sp=rl&se=2030-03-14T21:53:38Z&st=2019-03-14T13:53:38Z&spr=https&sig=looyudbytezOmD2iqCLiZ7xt%2F%2FajvQOF6PR8BULGmpI%3D";

        public static IEnumerable<PackageIdentifier> ListInstalledPackages()
        {
            foreach (var packageDir in Directory.EnumerateDirectories(Platform.PackageDirectory))
            {
                foreach (var packageVer in Directory.EnumerateDirectories(packageDir))
                {
                    yield return new PackageIdentifier { Name = Path.GetFileName(packageDir), Version = Version.Parse(Path.GetFileName(packageVer)) };
                }
            }
        }

        public static async Task<IList<string>> ListToolchains()
        {
            return await ListPackages("toolchain");
        }

        public static async Task<IList<string>> ListPackages(string type = "")
        {
            var result = new List<string>();

            if (CloudStorageAccount.TryParse(sharedAccessString, out var storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    var cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    var containers = await cloudBlobClient.ListContainersSegmentedAsync(new BlobContinuationToken());

                    foreach (var container in containers.Results)
                    {
                        await container.FetchAttributesAsync();

                        if (type == "" || container.Metadata["type"] == type)
                        {
                            result.Add(container.Name.Replace("-", "."));
                        }
                    }
                }
                catch
                {

                }
            }

            return result;
        }

        private static bool PlatformMatches(PackagePlatform platform)
        {
            return platform == PackagePlatform.Any || GetSystemPackagePlatform() == platform;
        }

        private static PackagePlatform GetSystemPackagePlatform()
        {
            var currentPlatform = Platform.AvalonRID;

            switch (currentPlatform)
            {
                case "win-x64":
                    return PackagePlatform.WinX64;

                case "osx-x64":
                    return PackagePlatform.Osx;

                case "linux-x64":
                    return PackagePlatform.LinuxX64;

                default:
                    return PackagePlatform.Unknown;
            }
        }

        public static async Task<IList<Package>> ListToolchainPackages(string toolchainName, bool includeAllPlatforms = false)
        {
            List<Package> result = null;

            if (CloudStorageAccount.TryParse(sharedAccessString, out var storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    var cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    var cloudBlobContainer = cloudBlobClient.GetContainerReference(toolchainName.Replace(".", "-").ToLower());

                    if (!await cloudBlobContainer.ExistsAsync())
                    {
                        return null;
                    }

                    var blobs = await cloudBlobContainer.ListBlobsSegmentedAsync(new BlobContinuationToken());

                    foreach (var blob in blobs.Results.OfType<CloudBlockBlob>())
                    {
                        await blob.FetchAttributesAsync();

                        var platform = Package.GetPackagePlatformFromString(blob.Metadata["platform"]);

                        if (includeAllPlatforms || PlatformMatches(platform))
                        {
                            var package = new Package();
                            package.Name = cloudBlobContainer.Name.Replace("-", ".");
                            package.BlobIdentity = blob.Name;

                            package.Platform = platform;
                            package.Version = Version.Parse(blob.Metadata["version"]);
                            package.Uri = blob.Uri;
                            package.Published = blob.Properties.Created.Value.UtcDateTime;
                            package.Size = blob.Properties.Length;

                            if (result == null)
                            {
                                result = new List<Package>();
                            }

                            result.Add(package);
                        }
                    }
                }
                catch { }
            }

            return result?.OrderByDescending(x => x.Version).ToList();
        }

        public static PackageManifest GetPackageManifest(string package, string version = null)
        {
            var directory = GetPackageDirectory(package, version);

            if (!string.IsNullOrWhiteSpace(directory) && File.Exists(Path.Combine(directory, "package.manifest")))
            {
                return PackageManifest.Load(Path.Combine(directory, "package.manifest"), package);
            }

            return null;
        }

        public static string GetPackageDirectory(string package, string version = null)
        {
            package = package.ToLower();
            
            var destinationPath = Path.Combine(Platform.PackageDirectory, package);

            if (Directory.Exists(destinationPath))
            {
                if (string.IsNullOrWhiteSpace(version))
                {
                    var versions = Directory.EnumerateDirectories(destinationPath);

                    if (versions.Count() == 0)
                    {
                        return "";
                    }

                    return Path.Combine(destinationPath, versions.FirstOrDefault());
                }
                else
                {
                    if (Directory.Exists(Path.Combine(destinationPath, version)))
                    {
                        return Path.Combine(destinationPath, version);
                    }
                }
            }

            return "";
        }


        public static async Task DownloadPackage(Package package, Action<long> progress)
        {
            if (CloudStorageAccount.TryParse(sharedAccessString, out var storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    var cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    var cloudBlobContainer = cloudBlobClient.GetContainerReference(package.Name.Replace(".", "-").ToLower());

                    if (!await cloudBlobContainer.ExistsAsync())
                    {
                        return;
                    }

                    var blob = cloudBlobContainer.GetBlockBlobReference(package.BlobIdentity);

                    var destinationPath = Path.Combine(Platform.PackageDirectory, package.Name, package.Version.ToString());

                    if (!Directory.Exists(destinationPath))
                    {
                        Directory.CreateDirectory(destinationPath);
                    }

                    long lastDownloadSize = 0;

                    await blob.DownloadToFileAsync(
                        Path.Combine(destinationPath, package.BlobIdentity),
                        FileMode.Create,
                        default(AccessCondition),
                        default(BlobRequestOptions),
                        default(OperationContext),
                        new Progress<StorageProgress>(p =>
                        {
                            if (p.BytesTransferred > lastDownloadSize + 256000)
                            {
                                progress(p.BytesTransferred);

                                lastDownloadSize = p.BytesTransferred;
                            }
                        }), CancellationToken.None);

                }
                catch
                {

                }
            }
        }

        public static void UnpackArchive(string archiveFileName, string targetDirectory, Action<long, long> progress, bool posixCompatibleExtraction, string password = null)
        {
            UnpackArchive(archiveFileName, targetDirectory, password != null ? ManagedLzma.PasswordStorage.Create(password) : null, progress, posixCompatibleExtraction);
        }

        private static void UnpackArchive(string archiveFileName, string targetDirectory, ManagedLzma.PasswordStorage password, Action<long, long> progress, bool posixCompatibleExtraction)
        {
            if (!File.Exists(archiveFileName))
                throw new FileNotFoundException("Archive not found.", archiveFileName);

            // Ensure that the target directory exists.
            Directory.CreateDirectory(targetDirectory);

            using (var archiveStream = new FileStream(archiveFileName, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete))
            {
                var archiveMetadataReader = new ManagedLzma.SevenZip.FileModel.ArchiveFileModelMetadataReader();
                archiveMetadataReader.EnablePosixFileAttributeExtension = posixCompatibleExtraction;
                var archiveFileModel = archiveMetadataReader.ReadMetadata(archiveStream, password);
                var archiveMetadata = archiveFileModel.Metadata;

                for (int sectionIndex = 0; sectionIndex < archiveMetadata.DecoderSections.Length; sectionIndex++)
                {
                    var sectionReader = new ManagedLzma.SevenZip.Reader.DecodedSectionReader(archiveStream, archiveMetadata, sectionIndex, password);

                    var sectionFiles = archiveFileModel.GetFilesInSection(sectionIndex);

                    // The section reader is constructed from metadata, if the counts do not match there must be a bug somewhere.
                    System.Diagnostics.Debug.Assert(sectionFiles.Count == sectionReader.StreamCount);

                    // The section reader iterates over all files in the section. NextStream advances the iterator.
                    for (; sectionReader.CurrentStreamIndex < sectionReader.StreamCount; sectionReader.NextStream())
                    {
                        var fileMetadata = sectionFiles[sectionReader.CurrentStreamIndex];

                        // The ArchiveFileModelMetadataReader we used above processes special marker nodes and resolves some conflicts
                        // in the archive metadata so we don't have to deal with them. In these cases there will be no file metadata
                        // produced and we should skip the stream. If you want to process these cases manually you should use a different
                        // MetadataReader subclass or write your own subclass.
                        if (fileMetadata == null)
                            continue;


                        // These asserts need to hold, otherwise there's a bug in the mapping the metadata reader produced.
                        System.Diagnostics.Debug.Assert(fileMetadata.Stream.SectionIndex == sectionIndex);
                        System.Diagnostics.Debug.Assert(fileMetadata.Stream.StreamIndex == sectionReader.CurrentStreamIndex);

                        // Ensure that the target directory is created.
                        var filename = Path.Combine(targetDirectory, fileMetadata.FullName);

                        Directory.CreateDirectory(Path.GetDirectoryName(filename));

                        // NOTE: you can have two using-statements here if you want to be explicit about it, but disposing the
                        //       stream provided by the section reader is not mandatory, it is owned by the the section reader
                        //       and will be auto-closed when moving to the next stream or when disposing the section reader.

                        var sourceStream = sectionReader.OpenStream();

                        ExtractTarByEntry(sourceStream, targetDirectory, false, progress);



                        //SetFileAttributes(filename, fileMetadata);
                    }
                }

                // Create empty files and empty directories.
                UnpackArchiveStructure(archiveFileModel.RootFolder, targetDirectory);
            }
        }

        private static void UnpackArchiveStructure(ManagedLzma.SevenZip.FileModel.ArchivedFolder folder, string targetDirectory)
        {
            if (folder.Items.IsEmpty)
            {
                // Empty folders need to be created manually since the unpacking code doesn't try to write into it.
                Directory.CreateDirectory(targetDirectory);
            }
            else
            {
                foreach (var item in folder.Items)
                {
                    var file = item as ManagedLzma.SevenZip.FileModel.ArchivedFile;
                    if (file != null)
                    {
                        // Files without content are not iterated during normal unpacking so we need to create them manually.
                        if (file.Stream.IsUndefined)
                        {
                            System.Diagnostics.Debug.Assert(file.Length == 0); // If the file has no content then its length should be zero, otherwise something is wrong.

                            var filename = Path.Combine(targetDirectory, file.Name);

                            try
                            {
                                using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Delete))
                                {
                                    // Nothing to do, FileMode.Create already truncates the file on opening.
                                }

                                SetFileAttributes(filename, file);
                            }
                            catch { }
                        }
                    }

                    var subfolder = item as ManagedLzma.SevenZip.FileModel.ArchivedFolder;
                    if (subfolder != null)
                        UnpackArchiveStructure(subfolder, Path.Combine(targetDirectory, subfolder.Name));
                }
            }
        }

        private static void SetFileAttributes(string path, ManagedLzma.SevenZip.FileModel.ArchivedFile file)
        {
            if (file.Attributes.HasValue)
            {
                // When calling File.SetAttributes we need to preserve existing attributes which are not part of the archive

                var attr = File.GetAttributes(path);
                const FileAttributes kAttrMask = ArchivedAttributesExtensions.FileAttributeMask;
                attr = (attr & ~kAttrMask) | (file.Attributes.Value.ToFileAttributes() & kAttrMask);
                File.SetAttributes(path, attr);
            }
        }

        public static void UnintallPackage(string package, string version, IConsole console = null)
        {
            var directory = PackageManager.GetPackageDirectory(package, version);

            if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
            {
                Directory.Delete(directory, true);

                console?.WriteLine($"Package {package} v{version} was uninstalled.");
            }
            else
            {
                console?.WriteLine($"Package {package} v{version} is not currently installed.");
            }
        }

        private static void ExtractTarByEntry(Stream inputStream, string targetDir, bool asciiTranslate, Action<long, long> progress)
        {
            TarInputStream tarIn = new TarInputStream(inputStream);
            TarEntry tarEntry;
            while ((tarEntry = tarIn.GetNextEntry()) != null)
            {
                if (tarEntry.IsDirectory)
                    continue;

                progress(tarIn.Position, tarIn.Length);

                // Converts the unix forward slashes in the filenames to windows backslashes
                string name = tarEntry.Name.Replace('/', Path.DirectorySeparatorChar);

                // Remove any root e.g. '\' because a PathRooted filename defeats Path.Combine
                if (Path.IsPathRooted(name))
                    name = name.Substring(Path.GetPathRoot(name).Length);

                // Apply further name transformations here as necessary
                string outName = Path.Combine(targetDir, name).NormalizePath();

                string directoryName = Path.GetDirectoryName(outName);

                // Does nothing if directory exists
                Directory.CreateDirectory(directoryName);

                if (tarEntry.TarHeader.TypeFlag != '0')
                {
                    switch ((char)tarEntry.TarHeader.TypeFlag)
                    {
                        case '1':
                            if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
                            {
                                Platform.CreateHardLinkWin32(outName.NormalizePath(), Path.Combine(targetDir, tarEntry.TarHeader.LinkName).NormalizePath(), !tarEntry.IsDirectory);
                            }
                            else
                            {
                                var symLinkInfo = new UnixSymbolicLinkInfo(Path.Combine(targetDir, tarEntry.TarHeader.LinkName).NormalizePath());

                                symLinkInfo.CreateLink(outName);
                            }
                            break;

                        case '2':
                            if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
                            {
                                Platform.CreateSymbolicLinkWin32(outName.NormalizePath(), tarEntry.TarHeader.LinkName, !tarEntry.IsDirectory);
                            }
                            else
                            {
                                var symLinkInfo = new UnixSymbolicLinkInfo(outName.NormalizePath());

                                symLinkInfo.CreateSymbolicLinkTo(tarEntry.TarHeader.LinkName);
                            }
                            break;
                    }
                }
                else
                {
                    try
                    {
                        using (var outStr = new FileStream(outName, FileMode.Create))
                        {

                            if (asciiTranslate)
                                CopyWithAsciiTranslate(tarIn, outStr);
                            else
                                tarIn.CopyEntryContents(outStr);
                        }

                        if (Platform.PlatformIdentifier == Platforms.PlatformID.Unix || Platform.PlatformIdentifier == Platforms.PlatformID.MacOSX)
                        {
                            var unixFileInfo = new UnixFileInfo(outName)
                            {
                                FileAccessPermissions = (FileAccessPermissions)tarEntry.TarHeader.Mode
                            };
                        }

                        // Set the modification date/time. This approach seems to solve timezone issues.
                        DateTime myDt = DateTime.SpecifyKind(tarEntry.ModTime, DateTimeKind.Utc);
                        File.SetLastWriteTime(outName, myDt);
                    }
                    catch (Exception)
                    {

                    }
                    }
            }

            tarIn.Close();
        }

        private static void CopyWithAsciiTranslate(TarInputStream tarIn, Stream outStream)
        {
            byte[] buffer = new byte[4096];
            bool isAscii = true;
            bool cr = false;

            int numRead = tarIn.Read(buffer, 0, buffer.Length);
            int maxCheck = Math.Min(200, numRead);
            for (int i = 0; i < maxCheck; i++)
            {
                byte b = buffer[i];
                if (b < 8 || (b > 13 && b < 32) || b == 255)
                {
                    isAscii = false;
                    break;
                }
            }

            while (numRead > 0)
            {
                if (isAscii)
                {
                    // Convert LF without CR to CRLF. Handle CRLF split over buffers.
                    for (int i = 0; i < numRead; i++)
                    {
                        byte b = buffer[i];     // assuming plain Ascii and not UTF-16
                        if (b == 10 && !cr)     // LF without CR
                            outStream.WriteByte(13);
                        cr = (b == 13);

                        outStream.WriteByte(b);
                    }
                }
                else
                    outStream.Write(buffer, 0, numRead);

                numRead = tarIn.Read(buffer, 0, buffer.Length);
            }
        }

        public static async Task InstallPackage(Package package, Action<long, long> progress)
        {
            await Task.Run(() =>
            {
                var archivePath = Path.Combine(Platform.PackageDirectory, package.Name, package.Version.ToString());
                
                UnpackArchive(Path.Combine(archivePath, package.BlobIdentity), archivePath, progress, true);

                File.Delete(Path.Combine(archivePath, package.BlobIdentity));
            });
        }

        public static Task<PackageEnsureStatus> EnsurePackage(string packageName, IConsole console)
        {
            return EnsurePackage(packageName, null, console);
        }

        public static async Task<PackageEnsureStatus> EnsurePackage(string packageName, string version, IConsole console)
        {
            packageName = packageName.ToLower();

            if (string.IsNullOrWhiteSpace(packageName))
            {
                console.WriteLine("Package with no name. Check definition in manifest file.");
                return PackageEnsureStatus.NotFound;
            }

            Version ver = string.IsNullOrWhiteSpace(version) ? null : Version.Parse(version);

            var packageDirectory = Path.Combine(Platform.PackageDirectory, packageName);

            if (ver == null && Directory.Exists(packageDirectory))
            {
                var versions = Directory.EnumerateDirectories(packageDirectory);

                if (versions.Count() != 0)
                {
                    ver = versions.Select(x => Version.Parse(Path.GetFileName(x))).OrderByDescending(x => x).FirstOrDefault();

                    packageDirectory = Path.Combine(Platform.PackageDirectory, packageName, ver.ToString());
                }
                else
                {
                    packageDirectory = Path.Combine(Platform.PackageDirectory, packageName, "0000");
                }
            }
            else if (ver == null)
            {
                packageDirectory = Path.Combine(Platform.PackageDirectory, packageName, "0000");
            }
            else
            {
                packageDirectory = Path.Combine(Platform.PackageDirectory, packageName, ver.ToString());
            }

            if (!Directory.Exists(packageDirectory))
            {
                var packages = await ListToolchainPackages(packageName, true);

                if (packages == null)
                {
                    console?.WriteLine($"Package {packageName} v{ver} was not found.");

                    return PackageEnsureStatus.NotFound;
                }

                if (ver == null)
                {
                    ver = packages.OrderByDescending(p => p.Version).FirstOrDefault().Version;
                }

                if (!packages.Any(p => p.Version == ver))
                {
                    console?.WriteLine($"Package {packageName} was found but version v{ver} not recognised. Lastest is: v{packages.First().Version}");

                    return PackageEnsureStatus.NotFound;
                }

                var systemPlatform = GetSystemPackagePlatform();

                if (!packages.Any(p => p.Platform == systemPlatform || p.Platform == PackagePlatform.Any))
                {
                    console?.WriteLine($"Package {packageName} v{ver} was found but is not supported on platform {Platform.AvalonRID}");

                    var supportedPlatforms = packages.GroupBy(p => p.Platform);

                    if (supportedPlatforms.Any())
                    {
                        console?.WriteLine($"Package {packageName} v{ver} supports:");
                        foreach (var platformPkg in supportedPlatforms)
                        {
                            console?.Write($"{platformPkg.Key} ");
                        }

                        console?.WriteLine();
                    }

                    return PackageEnsureStatus.NotFound;
                }

                console?.WriteLine($"Package: {packageName} v{ver} will be downloaded and installed.");

                var package = packages.FirstOrDefault(p => p.Version == ver && (p.Platform == systemPlatform || p.Platform == PackagePlatform.Any));

                await DownloadPackage(package, p =>
                {
                    console?.OverWrite($"Downloading: [{(((float)p / package.Size) * 100.0f).ToString("0.00")}%] {ByteSizeHelper.ToString(p)}/{ByteSizeHelper.ToString(package.Size)}     ");
                });

                console?.OverWrite($"Downloaded Package: {packageName} v{ver}.");
                console?.WriteLine();
                console?.WriteLine($"Extracting Package: {packageName} v{ver}.");

                await InstallPackage(package, (offset, length) => console.OverWrite($"Extracting: [{(((float)offset / length) * 100.0f).ToString("0.00")}%]"));

                await LoadAssetsAsync(Path.Combine(Platform.PackageDirectory, package.Name, package.Version.ToString()));

                await ResolveDependencies(packageName, ver?.ToString(), console);

                console?.OverWrite($"Package Installed: {packageName} v{ver}.");
                console?.WriteLine();

                return PackageEnsureStatus.Installed;
            }
            else
            {
                return await ResolveDependencies(packageName, ver?.ToString(), console);
            }
        }

        public static async Task<string> ResolvePackagePathAsync(string url, bool appendExecutableExtension = true, IConsole console = null)
        {
            string result = "";

            var packageInfo = ParseUrl(url);

            var fullPackageId = (packageInfo.package + packageInfo.version).ToLower();

            var packageLocation = "";

            if (await EnsurePackage(packageInfo.package, packageInfo.version, console) == PackageEnsureStatus.NotFound)
            {
                throw new Exception("Package not found.");
            }

            packageLocation = PackageManager.GetPackageDirectory(packageInfo.package, packageInfo.version).ToPlatformPath();

            result = (Path.Combine(packageLocation, packageInfo.location) + (appendExecutableExtension ? Platform.ExecutableExtension : "")).ToPlatformPath();

            return result;
        }

        public static (string package, string version, string location) ParseUrl(string url)
        {
            string location = "";
            string package = "";
            string version = "";

            if (url.Contains("?") || url.Contains("="))
            {
                var urlQueryOperatorIndex = url.IndexOf('?');

                if (urlQueryOperatorIndex == -1)
                {
                    urlQueryOperatorIndex = 0;
                }

                location = url.Substring(0, urlQueryOperatorIndex);

                var parameters = url.Substring(urlQueryOperatorIndex == 0 ? 0 : urlQueryOperatorIndex + 1, url.Length - (urlQueryOperatorIndex == 0 ? 0 : urlQueryOperatorIndex + 1)).Replace("?","");

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

            return (null, null, null);
        }

            public static async Task<PackageEnsureStatus> ResolveDependencies(string package, string packageVersion = null, IConsole console = null)
        {
            var manifest = GetPackageManifest(package, packageVersion);

            if (manifest != null && manifest.Properties.TryGetValue("Dependencies", out var dependencies))
            {
                console?.WriteLine($"Resolving Dependencies for {package} v{packageVersion}");

                foreach (var dependency in dependencies as JArray)
                {
                    var dep = manifest.ParseUrl(dependency.ToString());

                    var result = await EnsurePackage(dep.package, dep.version, console);

                    if(result == PackageEnsureStatus.NotFound || result == PackageEnsureStatus.Unknown)
                    {
                        return PackageEnsureStatus.NotFound;
                    }
                }
            }

            return PackageEnsureStatus.Found;
        }

        public static async Task LoadAssetsAsync()
        {
            foreach (var package in Directory.EnumerateDirectories(Platform.PackageDirectory))
            {
                foreach (var packageVersion in Directory.EnumerateDirectories(package))
                {
                    await LoadAssetsAsync(packageVersion);
                }
            }
        }

        private static async Task LoadAssetsAsync(string location)
        {
            var files = Directory.EnumerateFiles(location, "*.*", SearchOption.TopDirectoryOnly);

            await LoadAssetsFromFilesAsync(files);
        }

        private static List<IPackageAssetLoader> _assetLoaders = new List<IPackageAssetLoader>();

        public static void RegisterAssetLoader(IPackageAssetLoader loader)
        {
            if (!_assetLoaders.Contains(loader))
            {
                _assetLoaders.Add(loader);
            }
        }

        private static async Task LoadAssetsFromFilesAsync(IEnumerable<string> files)
        {
            foreach (var assetLoader in _assetLoaders)
            {
                await assetLoader.LoadAssetsAsync(files);
            }
        }
    }
}
