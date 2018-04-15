using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.Standard
{
    public abstract class StandardToolchain : IToolchain
    {
        private int buildCount;

        private int fileCount;
        private int numTasks;
        private IShell _shell;
        private IStatusBar _statusBar;

        protected IShell Shell => _shell;

        private readonly object resultLock = new object();

        private bool terminateBuild;

        protected StandardToolchain(IStatusBar statusBar)
        {
            _statusBar = statusBar;

            Jobs = Environment.ProcessorCount;
        }

        public int Jobs { get; set; }

        public abstract string ExecutableExtension { get; }
        public abstract string StaticLibraryExtension { get; }

        public abstract bool ValidateToolchainExecutables(IConsole console);

        private bool ExecuteCommands(IConsole console, IProject project, IList<string> commands)
        {
            bool result = true;

            foreach (var command in commands)
            {
                var commandParts = command.Split(' ');

                var cmd = commandParts[0];
                var args = command.Remove(0, cmd.Length).Trim();

                if (ExecuteCommand(console, project, cmd.ToPlatformPath(), args.ToPlatformPath()) != 0)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        private int ExecuteCommand(IConsole console, IProject project, string command, string args)
        {
            var environment = project.GetEnvironmentVariables().AppendRange(Platform.EnvironmentVariables);

            command = command.ExpandVariables(environment);
            args = args.ExpandVariables(environment);

            console.WriteLine($"[CMD] {command} {args}");

            var exitCode = PlatformSupport.ExecuteShellCommand(command, args, (s, e) =>
            {
                console.WriteLine(e.Data);
            }, (s, ee) =>
            {
                if (ee.Data != null)
                {
                    console.WriteLine(ee.Data);
                }
            }, false, project.CurrentDirectory, true, true, project.ToolChain?.BinDirectory);

            return exitCode;
        }

        public async Task<bool> BuildAsync(IConsole console, IProject project, string label = "", IEnumerable<string> defines = null)
        {
            try
            {
                if (!await InstallAsync(console, project))
                {
                    console.WriteLine("Failed: Unable to install or initialise toolchain.");
                    return false;
                }
            }
            catch (Exception e)
            {
                console.WriteLine("Failed: Unable to install or initialise toolchain. Due to an unexpected error.");
                console.WriteLine();

                console.WriteLine(e.Message);
                console.WriteLine(e.StackTrace);
            }

            console.Clear();

            console.WriteLine("Starting Build...");
            console.WriteLine();

            if (!ValidateToolchainExecutables(console))
            {
                console.WriteLine("Failed: Unable to find toolchain executables.");
                return false;
            }

            await BeforeBuild(console, project);

            var preBuildCommands = (project as IStandardProject).PreBuildCommands;
            var postBuildCommands = (project as IStandardProject).PostBuildCommands;

            bool result = true;

            if (preBuildCommands.Count > 0)
            {
                console.WriteLine("Pre-Build Commands:");

                result = ExecuteCommands(console, project, preBuildCommands);
            }

            terminateBuild = !result;

            SetFileCount(project as IStandardProject);
            buildCount = 0;

            var compiledProjects = new List<CompileResult>();

            List<Definition> injectedDefines = new List<Definition>();

            if (defines != null)
            {
                foreach (var define in defines)
                {
                    var injectableDefinition = new Definition() { Global = true, Value = define };
                    (project as IStandardProject).Defines.Add(injectableDefinition);
                    injectedDefines.Add(injectableDefinition);
                }
            }

            if (!terminateBuild)
            {
                await CompileProject(console, project as IStandardProject, project as IStandardProject, compiledProjects);

                if (!terminateBuild)
                {
                    await WaitForCompileJobs();

                    foreach (var compiledReference in compiledProjects)
                    {
                        result = compiledReference.ExitCode == 0;

                        if (!result)
                        {
                            break;
                        }
                    }

                    if (result)
                    {
                        var linkedReferences = new CompileResult();
                        linkedReferences.Project = project as IStandardProject;

                        foreach (var compiledProject in compiledProjects)
                        {
                            if (compiledProject.Project.Location != project.Location)
                            {
                                var linkResult = Link(console, project as IStandardProject, compiledProject, linkedReferences);
                            }
                            else
                            {
                                linkedReferences.ObjectLocations = compiledProject.ObjectLocations;
                                linkedReferences.NumberOfObjectsCompiled = compiledProject.NumberOfObjectsCompiled;
                                var linkResult = Link(console, project as IStandardProject, linkedReferences, linkedReferences, label);

                                console.WriteLine();

                                if (postBuildCommands.Count > 0)
                                {
                                    console.WriteLine("Post-Build Commands:");
                                    bool succeess = ExecuteCommands(console, project, postBuildCommands);

                                    if (!succeess)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }

                            if (linkedReferences.ExitCode != 0)
                            {
                                result = false;
                                break;
                            }
                        }
                    }

                    ClearBuildFlags(project as IStandardProject);
                }
            }

            console.WriteLine();

            if (terminateBuild)
            {
                result = false;
            }

            if (result)
            {
                console.WriteLine("Build Successful");
            }
            else
            {
                console.WriteLine("Build Failed");
            }

            foreach (var define in injectedDefines)
            {
                (project as IStandardProject).Defines.Remove(define);
            }

            project.Save();

            return result;
        }

        public async Task Clean(IConsole console, IProject project)
        {
            await Task.Factory.StartNew(async () =>
            {
                console.WriteLine("Starting Clean...");

                await CleanAll(console, project as IStandardProject, project as IStandardProject);

                console.WriteLine("Clean Completed.");
            });
        }

        public abstract IList<object> GetConfigurationPages(IProject project);

        public abstract bool CanHandle(IProject project);

        public string Name
        {
            get { return GetType().ToString(); }
        }

        public abstract Version Version { get; }

        public abstract string Description { get; }

        public abstract CompileResult Compile(IConsole console, IStandardProject superProject, IStandardProject project,
            ISourceFile file, string outputFile);

        public abstract LinkResult Link(IConsole console, IStandardProject superProject, IStandardProject project,
            CompileResult assemblies, string outputPath);

        public abstract ProcessResult Size(IConsole console, IStandardProject project, LinkResult linkResult);

        public abstract string GetCompilerArguments(IStandardProject superProject, IStandardProject project,
            ISourceFile sourceFile);

        public abstract string GetLinkerArguments(IStandardProject superProject, IStandardProject project);

        public abstract IEnumerable<string> GetToolchainIncludes(ISourceFile file);

        public abstract bool SupportsFile(ISourceFile file);

        /// <summary>
        /// This may be called very often so should not require large amounts of processing.
        /// </summary>
        public abstract string BinDirectory { get; }

        private void ClearBuildFlags(IStandardProject project)
        {
            foreach (var reference in project.References)
            {
                var standardReference = reference as IStandardProject;

                if (standardReference != null)
                {
                    ClearBuildFlags(standardReference);
                }
            }

            project.IsBuilding = false;
        }

        private int GetFileCount(IStandardProject project)
        {
            var result = 0;

            foreach (var reference in project.References)
            {
                var standardReference = reference as IStandardProject;

                if (standardReference != null)
                {
                    result += GetFileCount(standardReference);
                }
            }

            if (!project.IsBuilding)
            {
                project.IsBuilding = true;

                result += project.SourceFiles.Where(sf => SupportsFile(sf)).Count();
            }

            return result;
        }

        private void SetFileCount(IStandardProject project)
        {
            ClearBuildFlags(project);

            fileCount = GetFileCount(project);

            ClearBuildFlags(project);
        }

        private async Task WaitForCompileJobs()
        {
            await Task.Factory.StartNew(() =>
            {
                while (numTasks > 0)
                {
                    Thread.Sleep(10);
                }
            });
        }

        private LinkResult Link(IConsole console, IStandardProject superProject, CompileResult compileResult,
            CompileResult linkResults, string label = "")
        {
            var binDirectory = compileResult.Project.GetBinDirectory(superProject);

            if (!Directory.Exists(binDirectory))
            {
                Directory.CreateDirectory(binDirectory);
            }

            var outputLocation = binDirectory;

            var executable = Path.Combine(outputLocation, compileResult.Project.Name);

            if (!string.IsNullOrEmpty(label))
            {
                executable += string.Format("-{0}", label);
            }

            if (compileResult.Project.Type == ProjectType.StaticLibrary)
            {
                executable = Path.Combine(outputLocation, "lib" + compileResult.Project.Name);
                executable += StaticLibraryExtension;
            }
            else
            {
                executable += ExecutableExtension;
            }

            if (!Directory.Exists(outputLocation))
            {
                Directory.CreateDirectory(outputLocation);
            }

            var link = false;
            foreach (var objectFile in compileResult.ObjectLocations)
            {
                if (!System.IO.File.Exists(executable) || (System.IO.File.GetLastWriteTime(objectFile) > System.IO.File.GetLastWriteTime(executable)))
                {
                    link = true;
                    break;
                }
            }

            if (!link)
            {
                foreach (var library in compileResult.LibraryLocations)
                {
                    if (!System.IO.File.Exists(executable) || (System.IO.File.GetLastWriteTime(library) > System.IO.File.GetLastWriteTime(executable)))
                    {
                        link = true;
                        break;
                    }
                }
            }

            var linkResult = new LinkResult { Executable = executable };

            if (link)
            {
                _statusBar?.SetText($"Linking: {compileResult.Project.Name}");
                console.OverWrite(string.Format("[LL]    [{0}]", compileResult.Project.Name));
                linkResult = Link(console, superProject, compileResult.Project, compileResult, executable);
                _statusBar?.ClearText();
            }

            if (linkResult.ExitCode == 0)
            {
                if (compileResult.Project.Type == ProjectType.StaticLibrary)
                {
                    if (compileResult.ObjectLocations.Count > 0)
                    {
                        // This is where we have a libray with just headers.
                        linkResults.LibraryLocations.Add(executable);
                    }
                }
                else
                {
                    superProject.Executable = superProject.Location.MakeRelativePath(linkResult.Executable).ToAvalonPath();
                    superProject.Save();
                    console.WriteLine();
                    Size(console, compileResult.Project, linkResult);
                    linkResults.ExecutableLocations.Add(executable);
                }
            }
            else if (linkResults.ExitCode == 0)
            {
                linkResults.ExitCode = linkResult.ExitCode;
            }

            return linkResult;
        }

        private async Task CompileProject(IConsole console, IStandardProject superProject, IStandardProject project,
            List<CompileResult> results = null)
        {
            if (project.Type == ProjectType.Executable && superProject != project)
            {
                if (project.ToolChain == null)
                {
                    terminateBuild = true;

                    console.WriteLine($"Project: {project.Name} does not have a toolchain set.");
                }

                await project.ToolChain?.BuildAsync(console, project);
            }
            else
            {
                if (!terminateBuild)
                {
                    if (results == null)
                    {
                        results = new List<CompileResult>();
                    }

                    foreach (var reference in project.References)
                    {
                        var standardReference = reference as IStandardProject;

                        if (standardReference != null)
                        {
                            await CompileProject(console, superProject, standardReference, results);
                        }
                    }

                    var outputDirectory = project.GetOutputDirectory(superProject);

                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    var doWork = false;

                    lock (resultLock)
                    {
                        if (!project.IsBuilding)
                        {
                            project.IsBuilding = true;
                            doWork = true;
                        }
                    }

                    if (doWork)
                    {
                        var objDirectory = project.GetObjectDirectory(superProject);

                        if (!Directory.Exists(objDirectory))
                        {
                            Directory.CreateDirectory(objDirectory);
                        }

                        var compileResults = new CompileResult();
                        compileResults.Project = project;

                        results.Add(compileResults);

                        var tasks = new List<Task>();
                        
                        var sourceFiles = project.SourceFiles.ToList();

                        _statusBar?.SetText($"Building Project: {project.Name}");

                        foreach (var file in sourceFiles)
                        {
                            if (terminateBuild)
                            {
                                break;
                            }

                            if (SupportsFile(file))
                            {
                                var outputName = Path.ChangeExtension(file.Name, ".o");
                                var objectPath = Path.Combine(objDirectory, project.CurrentDirectory.MakeRelativePath(file.CurrentDirectory));
                                var objectFile = Path.Combine(objectPath, outputName);
                                var dependencyFile = Path.ChangeExtension(objectFile, ".d");

                                if (!Directory.Exists(objectPath))
                                {
                                    Directory.CreateDirectory(objectPath);
                                }

                                var dependencyChanged = false;

                                if (System.IO.File.Exists(dependencyFile))
                                {
                                    var dependencies = new List<string>();

                                    dependencies.Add(file.Location);
                                    dependencies.AddRange(ProjectExtensions.GetDependencies(dependencyFile));

                                    foreach (var dependency in dependencies)
                                    {
                                        if (!System.IO.File.Exists(dependency) || System.IO.File.GetLastWriteTime(dependency) > System.IO.File.GetLastWriteTime(objectFile))
                                        {
                                            dependencyChanged = true;
                                            break;
                                        }
                                    }
                                }

                                if (dependencyChanged || !System.IO.File.Exists(objectFile))
                                {
                                    while (numTasks >= Jobs)
                                    {
                                        Thread.Sleep(10);
                                    }

                                    lock (resultLock)
                                    {
                                        if (terminateBuild)
                                        {
                                            break;
                                        }
                                        
                                        numTasks++;
                                    }

                                    Task.Run(() =>
                                    {
                                        var stringBuilderConsole = new StringBuilderConsole();
                                        var compileResult = Compile(stringBuilderConsole, superProject, project, file, objectFile);

                                        lock (resultLock)
                                        {
                                            if (compileResults.ExitCode == 0 && compileResult.ExitCode != 0)
                                            {
                                                terminateBuild = true;
                                                compileResults.ExitCode = compileResult.ExitCode;
                                            }
                                            else
                                            {
                                                compileResults.ObjectLocations.Add(objectFile);
                                                compileResults.NumberOfObjectsCompiled++;
                                            }

                                            numTasks--;
                                        }

                                        console.OverWrite($"[CC {++buildCount}/{fileCount}]    [{project.Name}]    {file.Project.Location.MakeRelativePath(file.Location)}");

                                        var output = stringBuilderConsole.GetOutput();

                                        if (!string.IsNullOrEmpty(output))
                                        {
                                            console.WriteLine(output);
                                        }
                                    }).GetAwaiter();
                                }
                                else
                                {
                                    buildCount++;
                                    compileResults.ObjectLocations.Add(objectFile);
                                }
                            }
                        }

                        _statusBar?.ClearText();
                    }
                }
            }
        }

        private async Task CleanAll(IConsole console, IStandardProject superProject, IStandardProject project)
        {
            foreach (var reference in project.References)
            {
                var loadedReference = reference as IStandardProject;

                if (loadedReference != null)
                {
                    if (loadedReference.Type == ProjectType.Executable)
                    {
                        await CleanAll(console, loadedReference, loadedReference);
                    }
                    else
                    {
                        await CleanAll(console, superProject, loadedReference);
                    }
                }
            }

            var outputDirectory = project.GetObjectDirectory(superProject);

            var hasCleaned = false;

            if (Directory.Exists(outputDirectory))
            {
                hasCleaned = true;

                try
                {
                    Directory.Delete(outputDirectory, true);
                }
                catch (Exception)
                {
                }
            }

            outputDirectory = project.GetOutputDirectory(superProject);

            if (Directory.Exists(outputDirectory))
            {
                hasCleaned = true;

                try
                {
                    Directory.Delete(outputDirectory, true);
                }
                catch (Exception)
                {
                }
            }

            if (hasCleaned)
            {
                console.WriteLine(string.Format("[BB] - Cleaning Project - {0}", project.Name));
            }
        }

        public virtual async Task BeforeBuild(IConsole console, IProject project)
        {
            _shell = IoC.Get<IShell>();
        }
          
        public abstract Task<bool> InstallAsync(IConsole console, IProject project);

        public Task InstallAsync(IConsole console)
        {
            return Task.CompletedTask;
        }
    }
}