using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Packaging;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains.Standard;
using CommandLine;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace AvalonStudio
{
    internal class Program
    {
        private const string version = "2.0.0.0";
        private const string releaseName = "Apollo";

        private static readonly ProgramConsole console = new ProgramConsole();

        private static ISolution LoadSolution(ProjectOption options)
        {
            var currentDir = Directory.GetCurrentDirectory();

            var solutionFile = Path.Combine(currentDir, options.Solution);

            if (System.IO.File.Exists(solutionFile))
            {
                return VisualStudioSolution.Load(solutionFile);
            }

            throw new Exception("Solution file: " + options.Solution + "could not be found.");
        }

        private static IProject FindProject(ISolution solution, string project)
        {
            try
            {
                return solution.FindProject(project);
            }
            catch (Exception e)
            {
                console.WriteLine(e.Message);
                return null;
            }
        }

        private static int RunList(ListOptions options)
        {
            switch(options.Command)
            {
                case "packages":
                    var packages = PackageManager.ListPackages().GetAwaiter().GetResult();

                    foreach(var package in packages)
                    {
                        console.WriteLine(package);
                    }
                    break;

                case "package-info":
                    if (!string.IsNullOrEmpty(options.Parameter))
                    {
                        var packageVersions = PackageManager.ListToolchainPackages(options.Parameter).GetAwaiter().GetResult();

                        foreach(var version in packageVersions)
                        {
                            console.WriteLine($"{version.Name}, {version.Version}, {ByteSizeHelper.ToString(version.Size)}, {version.Published.ToUniversalTime()}");
                        }

                        return 1;
                    }
                    else
                    {
                        console.WriteLine("package name needs to be provided.");
                    }
                    break;

                case "toolchains":
                    packages = PackageManager.ListToolchains().GetAwaiter().GetResult();

                    foreach (var package in packages)
                    {
                        var packageVersions = PackageManager.ListToolchainPackages(package).GetAwaiter().GetResult();

                        if (packageVersions.Any())
                        {
                            var version = packageVersions.First();
                            console.WriteLine($"{version.Name}, {version.Version}, {ByteSizeHelper.ToString(version.Size)}, {version.Published.ToUniversalTime()}");
                        }
                    }

                    return 1;
            }

            return 2;
        }

        private static int RunTest(TestOptions options)
        {
            var result = 1;
            var solution = LoadSolution(options);

            solution.LoadSolutionAsync().Wait();
            solution.LoadProjectsAsync().Wait();

            var tests = new List<Test>();

            foreach (var project in solution.Projects)
            {
                if (project.TestFramework != null)
                {
                    var buildTask = project.ToolChain.BuildAsync(console, project, "");

                    buildTask.Wait();

                    if (buildTask.Result)
                    {
                        var awaiter = project.TestFramework.EnumerateTestsAsync(project);
                        awaiter.Wait();

                        foreach (var test in awaiter.Result)
                        {
                            tests.Add(test);
                        }
                    }
                    else
                    {
                        result = 2;
                    }
                }
            }

            foreach (var test in tests)
            {
                test.Run();

                if (test.Pass)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    console.Write("\x1b[32;1m");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    console.Write("\x1b[31;1m");
                }

                console.WriteLine(string.Format("Running Test: [{0}], [{1}]", test.Name, test.Pass ? "Passed" : "Failed"));

                if (!test.Pass)
                {
                    console.WriteLine();
                    console.WriteLine(string.Format("Assertion = [{0}], File=[{1}], Line=[{2}]", test.Assertion, test.File, test.Line));
                    console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.White;
                console.Write("\x1b[39; 49m");

                if (!test.Pass)
                {
                    result = 0;
                }
            }

            return result;
        }

        private static int RunInstall (InstallOptions options)
        {
            var result = PackageManager.EnsurePackage(options.PackageName, options.Version, console).GetAwaiter().GetResult();

            switch (result)
            {
                case PackageEnsureStatus.Found:
                case PackageEnsureStatus.Installed:
                    return 1;

                default: return 2;
            }
        }

        private static int RunUninstall(UninstallOptions options)
        {
            PackageManager.UnintallPackage(options.PackageName, options.Version, console);

            return 1;
        }

        private static int RunPrintEnv(PrintEnvironmentOptions options)
        {
            var manifest = PackageManager.GetPackageManifest(options.PackageName, options.Version);

            if (manifest != null)
            {
                console.WriteLine();

                if (manifest.Properties.TryGetValue("Paths", out var paths))
                {
                    var concatenatedPath = "";
                    foreach (var path in paths as JArray)
                    {
                        concatenatedPath += manifest.ResolvePackagePath(path.ToString(), false) + ";";
                    }
                    console.WriteLine("Path: " + concatenatedPath);

                    console.WriteLine();
                }

                if (manifest.Properties.TryGetValue("EnvironmentVariables", out var variables))
                {
                    console.WriteLine("Environment Variables:");

                    foreach(var variable in (variables as JObject))
                    {
                        console.WriteLine($"{variable.Key}={variable.Value}");
                    }

                    if (manifest.Properties.TryGetValue("Gcc.CC", out var cc))
                    {
                        console.WriteLine($"CC={manifest.ResolvePackagePath(cc.ToString())}");
                    }

                    if (manifest.Properties.TryGetValue("Gcc.CXX", out var cxx))
                    {
                        console.WriteLine($"CXX={manifest.ResolvePackagePath(cxx.ToString())}");
                    }

                    if (manifest.Properties.TryGetValue("Gcc.AR", out var ar))
                    {
                        console.WriteLine($"AR={manifest.ResolvePackagePath(ar.ToString())}");
                    }

                    if (manifest.Properties.TryGetValue("Gcc.LD", out var ld))
                    {
                        console.WriteLine($"LD={manifest.ResolvePackagePath(ld.ToString())}");
                    }

                    if (manifest.Properties.TryGetValue("Gcc.SIZE", out var size))
                    {
                        console.WriteLine($"SIZE={manifest.ResolvePackagePath(size.ToString())}");
                    }

                    if (manifest.Properties.TryGetValue("Gcc.GDB", out var gdb))
                    {
                        console.WriteLine($"GDB={manifest.ResolvePackagePath(gdb.ToString())}");
                    }

                    console.WriteLine();
                }

                if (manifest.Properties.TryGetValue("Gcc.SystemIncludePaths", out var systemIncludePaths))
                {
                    string includeFlags = "";
                    foreach (var unresolvedPath in (systemIncludePaths as JArray).ToList().Select(x => x.ToString()))
                    {
                        includeFlags += " -Isystem " + manifest.ResolvePackagePath(unresolvedPath, false);
                    }

                    console.WriteLine("CCFLAGS=" + includeFlags);

                    console.WriteLine();

                    console.WriteLine("CXXFLAGS=" + includeFlags);

                    console.WriteLine();
                }

                if (manifest.Properties.TryGetValue("Gcc.SystemLibraryPaths", out var systemLibraryPaths))
                {
                    string includeFlags = "";
                    foreach (var unresolvedPath in (systemLibraryPaths as JArray).ToList().Select(x => x.ToString()))
                    {
                        includeFlags += " -L " + manifest.ResolvePackagePath(unresolvedPath, false);
                    }

                    console.WriteLine("LDFLAGS=" + includeFlags);

                    console.WriteLine();
                }
            }
            return 1;
        }

        private static int RunCreatePackage(CreatePackageOptions options)
        {
            if (CloudStorageAccount.TryParse(options.ConnectionString, out var storageAccount))
            {
                try
                {
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    var cloudBlobContainer = cloudBlobClient.GetContainerReference(options.PackageName.Replace(".", "-").ToLower());

                    if (cloudBlobContainer.ExistsAsync().GetAwaiter().GetResult())
                    {
                        console.WriteLine($"Package: {options.PackageName} is already taken.");

                        return 2;
                    }
                    else
                    {
                        if (cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, default(BlobRequestOptions), default(OperationContext)).GetAwaiter().GetResult())
                        {
                            cloudBlobContainer.Metadata["type"] = options.Type;
                            cloudBlobContainer.SetMetadataAsync().Wait();

                            console.WriteLine($"Package: {options.PackageName} created.");
                            return 1;
                        }

                        console.WriteLine($"Package: {options.PackageName} is already taken.");
                        return 2;
                    }
                }
                catch (Exception e)
                {
                    console.WriteLine("Package could not be created. " + e.Message);

                    return 2;
                }
            }
            else
            {
                console.WriteLine("Invalid connection string");

                return 2;
            }
        }

        private static int RunPushPackage(PushPackageOptions options)
        {
            if (CloudStorageAccount.TryParse(options.ConnectionString, out var storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    var cloudBlobContainer = cloudBlobClient.GetContainerReference(options.PackageName.Replace(".", "-").ToLower());

                    if(!cloudBlobContainer.ExistsAsync().GetAwaiter().GetResult())
                    {
                        console.WriteLine($"Package: {options.PackageName} does not exist.");
                        return 2;
                    }

                    if(!File.Exists(options.File))
                    {
                        console.WriteLine($"File not found: {options.File}");
                        return 2;
                    }

                    switch(options.Platform)
                    {
                        case "any":
                        case "win-x64":
                        case "osx-x64":
                        case "linux-x64":
                            break;

                        default:
                            console.WriteLine($"Platform {options.Platform} is not valid.");
                            return 2;
                    }

                    var ver = Version.Parse(options.Version);

                    // Get a reference to the blob address, then upload the file to the blob.
                    // Use the value of localFileName for the blob name.
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Path.GetFileName(options.File));
                    var fileInfo = new FileInfo(options.File);

                    var progress = new Progress<StorageProgress>(p =>
                    {
                        console.WriteLine($"Uploaded: {p.BytesTransferred} / {fileInfo.Length}");
                    });

                    cloudBlockBlob.Metadata["platform"] = options.Platform;
                    cloudBlockBlob.Metadata["version"] = ver.ToString();

                    cloudBlockBlob.UploadFromFileAsync(options.File, default(AccessCondition), default(BlobRequestOptions), default(OperationContext), progress, new System.Threading.CancellationToken());

                    console.WriteLine($"Package uploaded: {cloudBlockBlob.Uri}");

                    return 1;
                }
                catch (Exception e)
                {
                    console.WriteLine("Error: " + e.Message);
                    return 2;
                }
            }
            else
            {
                console.WriteLine("Invalid connection string");

                return 2;
            }
        }

        private static int RunBuild(BuildOptions options)
        {
            var result = 1;
            var solution = LoadSolution(options);

            solution.LoadSolutionAsync().Wait();
            solution.LoadProjectsAsync().Wait();

            IProject project = null;

            if (options.Project != null)
            {
                project = FindProject(solution, options.Project);
            }
            else
            {
                project = solution.StartupProject;
            }

            if (project != null)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                if (project.ToolChain is StandardToolchain)
                {
                    (project.ToolChain as StandardToolchain).Jobs = options.Jobs;
                }

                var awaiter = project.ToolChain.BuildAsync(console, project, options.Label, options.Defines);
                awaiter.Wait();

                stopWatch.Stop();
                console.WriteLine(stopWatch.Elapsed.ToString());

                result = awaiter.Result ? 1 : 2;
            }
            else
            {
                console.WriteLine("Nothing to build.");
            }

            return result;
        }

        private static int RunClean(CleanOptions options)
        {
            var solution = LoadSolution(options);

            solution.LoadSolutionAsync().Wait();
            solution.LoadProjectsAsync().Wait();

            var console = new ProgramConsole();

            IProject project = null;

            if (options.Project != null)
            {
                project = FindProject(solution, options.Project);
            }
            else
            {
                project = solution.StartupProject;
            }

            if (project != null)
            {
                project.ToolChain.Clean(console, project).Wait();
            }
            else
            {
                console.WriteLine("Nothing to clean.");
            }

            return 1;
        }

        private static int RunRemove(RemoveOptions options)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), options.File);

            if (System.IO.File.Exists(file))
            {
                var solution = LoadSolution(options);
                var project = FindProject(solution, options.Project);

                if (project != null)
                {
                    // todo normalize paths.
                    var currentFile =
                        project.Items.OfType<ISourceFile>().Where(s => s.FilePath.Normalize() == options.File.Normalize()).FirstOrDefault();

                    if (currentFile != null)
                    {
                        project.Items.RemoveAt(project.Items.IndexOf(currentFile));
                        project.Save();

                        Console.WriteLine("File removed.");

                        return 1;
                    }
                    Console.WriteLine("File not found in project.");
                    return -1;
                }
                Console.WriteLine("Project not found.");
                return -1;
            }
            Console.WriteLine("File not found.");
            return -1;
        }

        private static int RunAdd(AddOptions options)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), options.File);

            if (System.IO.File.Exists(file))
            {
                var solution = LoadSolution(options);
                var project = FindProject(solution, options.Project) as CPlusPlusProject;

                if (project != null)
                {
                    throw new NotImplementedException();

                    /*var sourceFile = SourceFile.FromPath(project, project, options.File);
                    project.Items.Add(sourceFile);
                    project.SourceFiles.InsertSorted(sourceFile);
                    project.Save();
                    Console.WriteLine("File added.");
                    return 1;*/
                }
                Console.WriteLine("Project not found.");
                return -1;
            }
            Console.WriteLine("File not found.");
            return -1;
        }

        private static int RunAddReference(AddReferenceOptions options)
        {
            var solution = LoadSolution(options);
            var project = FindProject(solution, options.Project) as CPlusPlusProject;

            if (project != null)
            {
                var currentReference = project.References.Where(r => r.Name == options.Name).FirstOrDefault();

                if (currentReference != null)
                {
                    project.UnloadedReferences[project.References.IndexOf(currentReference)] = new Reference
                    {
                        Name = options.Name,
                        GitUrl = options.GitUrl,
                        Revision = options.Revision
                    };
                    Console.WriteLine("Reference successfully updated.");
                }
                else
                {
                    var add = true;

                    if (string.IsNullOrEmpty(options.GitUrl))
                    {
                        var reference = FindProject(solution, options.Name);

                        if (reference == null)
                        {
                            add = false;
                        }
                    }

                    if (add)
                    {
                        project.UnloadedReferences.Add(new Reference
                        {
                            Name = options.Name,
                            GitUrl = options.GitUrl,
                            Revision = options.Revision
                        });
                        Console.WriteLine("Reference added successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Local reference does not exist, try creating the project first.");
                    }
                }

                project.Save();
            }

            return 1;
        }

        private static int RunCreate(CreateOptions options)
        {
            var projectPath = string.Empty;

            if (string.IsNullOrEmpty(options.Project))
            {
                projectPath = Directory.GetCurrentDirectory();
                options.Project = Path.GetFileNameWithoutExtension(projectPath);
            }
            else
            {
                projectPath = Path.Combine(Directory.GetCurrentDirectory(), options.Project);
            }

            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }

            throw new NotImplementedException();
            var project = CPlusPlusProject.Create(projectPath, options.Project);

            if (project != null)
            {
                Console.WriteLine("Project created successfully.");
                return 1;
            }
            Console.WriteLine("Unable to create project. May already exist.");
            return -1;
        }

        private static int Main(string[] args)
        {
            var manifest = new PackageManifest();

            manifest.Properties["Dependencies"] = new List<string> {
                "Rpi.Gcc&Version=5.6.7.1",
                "Jlink&Version=4.5.1.2"
            };

            manifest.Properties["Gcc.CC"] = "bin/arm-linux-gnueabihf-gcc";
            manifest.Properties["Gcc.CXX"] = "bin/arm-linux-gnueabihf-g++";
            manifest.Properties["Gcc.AR"]= "bin/arm-linux-gnueabihf-ar";
            manifest.Properties["Gcc.LD"]= "bin/arm-linux-gnueabihf-gcc";
            manifest.Properties["Gcc.SIZE"]= "bin/arm-linux-gnueabihf-size";
            manifest.Properties["Gcc.GDB"]= "bin/arm-linux-gnueabihf-gdb";

            manifest.Properties["Paths"] = new List<string> { "bin/" };

            manifest.Properties["EnvironmentVariables"] = new Dictionary<string, string>
            {
                { "SYSROOT", "sysroot/include" }
            };

            manifest.Properties["Gcc.SystemIncludePaths"] = new List<string>
            {
                "arm-linux-gnueabihf/include/c++/7.3.0",
                "arm-linux-gnueabihf/include/c++/7.3.0/arm-linux-gnueabihf",
                "arm-linux-gnueabihf/include/c++/7.3.0/backward",
                "lib/gcc/arm-linux-gnueabihf/7.3.0/include",
                "lib/gcc/arm-linux-gnueabihf/7.3.0/include-fixed",
                "arm-linux-gnueabihf/include",
                "arm-linux-gnueabihf/usr/include",
                "usr/include"
            };

            manifest.Properties["Gcc.SystemLibraryPaths"] = new List<string>{
                "lib/arm-linux-gnueabihf"
            };

            manifest.Save("package.manifest");

            if(args.Length >= 1 && args[0] == "debug")
            {
                Console.WriteLine("Waiting for debugger to attach.");

                while(!Debugger.IsAttached)
                {
                    Thread.Sleep(100);
                }

                Debugger.Break();

                args = args.ToList().Skip(1).ToArray();
            }

            AvalonStudio.Shell.Extensibility.Platforms.Platform.AppName = "AvalonStudio";
            AvalonStudio.Shell.Extensibility.Platforms.Platform.Initialise();

            Platform.Initialise();

            var extensionManager = new ExtensionManager();
            var container = CompositionRoot.CreateContainer(extensionManager);

            var shell = container.GetExport<IShell>();
            
            IoC.Initialise(container);

            ShellViewModel.Instance = IoC.Get<ShellViewModel>();

            ShellViewModel.Instance.Initialise();

            PackageManager.LoadAssetsAsync().Wait();

            Console.WriteLine("Avalon Build - {0} - {1}  - {2}", releaseName, version, Platform.PlatformIdentifier);

            var result = Parser.Default.ParseArguments<
                AddOptions, 
                RemoveOptions, 
                AddReferenceOptions, 
                BuildOptions, 
                CleanOptions, 
                CreateOptions, 
                PackageOptions, 
                TestOptions, 
                ListOptions,
                InstallOptions,
                UninstallOptions,
                PrintEnvironmentOptions,
                CreatePackageOptions,
                PushPackageOptions>(args)
                .MapResult((BuildOptions opts) => RunBuild(opts),
                        (AddOptions opts) => RunAdd(opts),
                        (AddReferenceOptions opts) => RunAddReference(opts),
                        (CleanOptions opts) => RunClean(opts),
                        (CreateOptions opts) => RunCreate(opts),
                        (RemoveOptions opts) => RunRemove(opts),
                        (TestOptions opts) => RunTest(opts),
                        (ListOptions opts) => RunList(opts),
                        (InstallOptions opts)=> RunInstall(opts),
                        (UninstallOptions opts)=> RunUninstall(opts),
                        (PrintEnvironmentOptions opts)=>RunPrintEnv(opts),
                        (CreatePackageOptions opts)=>RunCreatePackage(opts),
                        (PushPackageOptions opts)=>RunPushPackage(opts),
                        errs => 1);

            return result - 1;
        }
    }
}