using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Repositories;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.Utils;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AvalonStudio
{
    internal class Program
    {
        private const string version = "1.2.0.0";
        private const string releaseName = "Gravity Waves";

        private static readonly ProgramConsole console = new ProgramConsole();

        private static Solution LoadSolution(ProjectOption options)
        {
            var currentDir = Directory.GetCurrentDirectory();

            var solutionFile = Path.Combine(currentDir, options.Solution);

            if (System.IO.File.Exists(solutionFile))
            {
                return Solution.Load(solutionFile);
            }

            throw new Exception("Solution file: " + options.Solution + "could not be found.");
        }

        private static IProject FindProject(Solution solution, string project)
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

        private static int RunInstallPackage(PackageOptions options)
        {
            console.Write("Downloading catalogs...");

            var availablePackages = new List<PackageReference>();

            foreach (var packageSource in PackageSources.Instance.Sources)
            {
                RepositoryOld repo = null;

                var awaiter = packageSource.DownloadCatalog();

                awaiter.Wait();

                repo = awaiter.Result;

                console.WriteLine("Done");

                console.WriteLine("Enumerating Packages...");

                if (repo != null)
                {
                    foreach (var packageReference in repo.Packages)
                    {
                        availablePackages.Add(packageReference);
                        console.WriteLine(packageReference.Name);
                    }
                }
            }

            var package = availablePackages.FirstOrDefault(p => p.Name == options.Package);

            if (package != null)
            {
                var task = package.DownloadInfoAsync();
                task.Wait();

                var repo = task.Result;

                var downloadTask = repo.Synchronize(options.Tag, console);
                downloadTask.Wait();

                return 1;
            }
            console.WriteLine("Unable to find package " + options.Package);
            return -1;
        }

        private static int RunTest(TestOptions options)
        {
            var result = 1;
            var solution = LoadSolution(options);

            var tests = new List<Test>();

            foreach (var project in solution.Projects)
            {
                if (project.TestFramework != null)
                {
                    var buildTask = project.ToolChain.Build(console, project, "");

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

        private static int RunBuild(BuildOptions options)
        {
            var result = 1;
            var solution = LoadSolution(options);

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

                if (project.ToolChain is StandardToolChain)
                {
                    (project.ToolChain as StandardToolChain).Jobs = options.Jobs;
                }

                var awaiter = project.ToolChain.Build(console, project, options.Label, options.Defines);
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
                    var sourceFile = SourceFile.FromPath(project, project, options.File);
                    project.Items.Add(sourceFile);
                    project.SourceFiles.InsertSorted(sourceFile);
                    project.Save();
                    Console.WriteLine("File added.");
                    return 1;
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
            var project = CPlusPlusProject.Create(null, projectPath, options.Project);

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
            Platform.Initialise();

            PackageSources.InitialisePackageSources();

            var container = CompositionRoot.CreateContainer();

            MinimalShell.Instance = container.GetExport<MinimalShell>();

            Console.WriteLine("Avalon Build - {0} - {1}  - {2}", releaseName, version, Platform.PlatformIdentifier);

            var result = Parser.Default.ParseArguments<AddOptions, RemoveOptions, AddReferenceOptions, BuildOptions, CleanOptions, CreateOptions, PackageOptions, TestOptions>(args)
                .MapResult((BuildOptions opts) => RunBuild(opts),
                        (AddOptions opts) => RunAdd(opts),
                        (AddReferenceOptions opts) => RunAddReference(opts),
                        (PackageOptions opts) => RunInstallPackage(opts),
                        (CleanOptions opts) => RunClean(opts),
                        (CreateOptions opts) => RunCreate(opts),
                        (RemoveOptions opts) => RunRemove(opts),
                        (TestOptions opts) => RunTest(opts),
                        errs => 1);

            return result - 1;
        }
    }
}