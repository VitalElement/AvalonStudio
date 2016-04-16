namespace AvalonStudio
{
    using System;
    using System.IO;
    using CommandLine;
    using System.Linq;
    using AvalonStudio.Projects;
    using AvalonStudio.Projects.CPlusPlus;
    using AvalonStudio.Toolchains;
    using AvalonStudio.Toolchains.STM32;
    using AvalonStudio.Projects.Standard;
    using AvalonStudio.Toolchains.Llilum;
    using Extensibility;
    using Toolchains.Standard;
    using Utils;
    using Platforms;
    using Repositories;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using TestFrameworks;
    class Program
    {
        const string version = "1.0.0.24";
        const string releaseName = "Gravity";

        const string baseDir = @"c:\development\vebuild\test";

        static ProgramConsole console = new ProgramConsole();

        static Solution LoadSolution(ProjectOption options)
        {
            var currentDir = Directory.GetCurrentDirectory();

            var solutionFile = Path.Combine(currentDir, options.Solution);

            if (File.Exists(solutionFile))
            {
                return Solution.Load(solutionFile);
            }

            throw new Exception("Solution file: " + options.Solution + "could not be found.");
        }

        static IProject FindProject(Solution solution, string project)
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


        static int RunInstallPackage(PackageOptions options)
        {
            console.Write("Downloading catalogs...");

            var availablePackages = new List<PackageReference>();

            foreach (var packageSource in PackageSources.Instance.Sources)
            {
                Repository repo = null;

                repo = packageSource.DownloadCatalog();
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

                var dlTask = repo.Synchronize(options.Tag, console);
                dlTask.Wait();

                return 1;

            }
            else
            {
                console.WriteLine("Unable to find package " + options.Package);
                return -1;
            }
        }

        static int RunTest(TestOptions options)
        {
            int result = 1;
            var solution = LoadSolution(options);

            var tests = new List<Test>();

            foreach (var project in solution.Projects)
            {
                if(project.TestFramework != null)
                {
                    project.ToolChain.Build(console, project, "").Wait();                    

                    var awaiter = project.TestFramework.EnumerateTestsAsync(project);
                    awaiter.Wait();

                    foreach(var test in awaiter.Result)
                    {
                        tests.Add(test);
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
                    console.WriteLine(string.Format("Assertion = [{0}], File=[{1}], Line=[{2}]", test.Assertion, test.File, test.Line));
                }

                Console.ForegroundColor = ConsoleColor.White;
                console.Write("\x1b[39; 49m");

                if (!test.Pass)
                {
                    result = 0;
                    break;
                }
            }

            return result;
        }

        static int RunBuild(BuildOptions options)
        {
            int result = 1;
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
                var stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();

                if (project.ToolChain is StandardToolChain)
                {
                    (project.ToolChain as StandardToolChain).Jobs = options.Jobs;
                }

                var awaiter = project.ToolChain.Build(console, project, options.Label);
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

        static int RunClean(CleanOptions options)
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

        static int RunRemove(RemoveOptions options)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), options.File);

            if (File.Exists(file))
            {
                var solution = LoadSolution(options);
                var project = FindProject(solution, options.Project);

                if (project != null)
                {
                    // todo normalize paths.
                    var currentFile = project.Items.OfType<ISourceFile>().Where((s) => s.File.Normalize() == options.File.Normalize()).FirstOrDefault();

                    if (currentFile != null)
                    {
                        project.Items.RemoveAt(project.Items.IndexOf(currentFile));
                        project.Save();

                        Console.WriteLine("File removed.");

                        return 1;
                    }
                    else
                    {
                        Console.WriteLine("File not found in project.");
                        return -1;
                    }

                }
                else
                {
                    Console.WriteLine("Project not found.");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("File not found.");
                return -1;
            }
        }

        static int RunAdd(AddOptions options)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), options.File);

            if (File.Exists(file))
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
                else
                {
                    Console.WriteLine("Project not found.");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("File not found.");
                return -1;
            }
        }

        static int RunAddReference(AddReferenceOptions options)
        {
            var solution = LoadSolution(options);
            var project = FindProject(solution, options.Project) as CPlusPlusProject;

            if (project != null)
            {
                var currentReference = project.References.Where((r) => r.Name == options.Name).FirstOrDefault();

                if (currentReference != null)
                {
                    project.UnloadedReferences[project.References.IndexOf(currentReference)] = new Reference { Name = options.Name, GitUrl = options.GitUrl, Revision = options.Revision };
                    Console.WriteLine("Reference successfully updated.");
                }
                else
                {
                    bool add = true;

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
                        project.UnloadedReferences.Add(new Reference { Name = options.Name, GitUrl = options.GitUrl, Revision = options.Revision });
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

        static int RunCreate(CreateOptions options)
        {
            string projectPath = string.Empty;

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
            else
            {
                Console.WriteLine("Unable to create project. May already exist.");
                return -1;
            }
        }

        static UInt32 PackValues(UInt16 a, UInt16 b)
        {
            return (UInt32)(a << 16 | b);
        }

        static void UnpackValues(UInt32 input, out UInt16 a, out UInt16 b)
        {
            a = (UInt16)(input >> 16);
            b = (UInt16)(input & 0x00FF);
        }

        static int Main(string[] args)
        {
            Platforms.Platform.Initialise();

            PackageSources.InitialisePackageSources();

            var container = CompositionRoot.CreateContainer();

            Shell.Instance = container.GetExportedValue<Shell>();

            var packed = PackValues(3, 4);

            UInt16 a = 0;
            UInt16 b = 0;

            UnpackValues(packed, out a, out b);

            packed = PackValues(a, b);


            Console.WriteLine(string.Format("Avalon Build - {0} - {1}  - {2}", releaseName, version, Platforms.Platform.PlatformIdentifier.ToString()));

            var result = Parser.Default.ParseArguments<AddOptions, RemoveOptions, AddReferenceOptions, BuildOptions, CleanOptions, CreateOptions, PackageOptions, TestOptions>(args).MapResult(
              (BuildOptions opts) => RunBuild(opts),
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
