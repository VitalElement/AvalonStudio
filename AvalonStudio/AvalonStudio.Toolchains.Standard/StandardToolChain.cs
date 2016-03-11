namespace AvalonStudio.Toolchains.Standard
{
    using AvalonStudio.Toolchains;
    using Platform;
    using Perspex.Controls;
    using Projects;
    using Projects.Standard;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Utils;

    public class ProcessResult
    {
        public int ExitCode { get; set; }
    }

    public class CompileResult : ProcessResult
    {
        public CompileResult()
        {
            ObjectLocations = new List<string>();
            LibraryLocations = new List<string>();
            ExecutableLocations = new List<string>();
        }

        public IStandardProject Project { get; set; }
        public List<string> ObjectLocations { get; set; }
        public List<string> LibraryLocations { get; set; }
        public List<string> ExecutableLocations { get; set; }
        public int NumberOfObjectsCompiled { get; set; }

        public int Count
        {
            get
            {
                return ObjectLocations.Count + LibraryLocations.Count + ExecutableLocations.Count;
            }
        }
    }

    public class LinkResult : ProcessResult
    {
        public string Executable { get; set; }
    }

    public abstract class StandardToolChain : IToolChain
    {
        public StandardToolChain()
        {
            this.Jobs = 4;
        }

        public int Jobs { get; set; }
        
        public abstract CompileResult Compile(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string outputFile);

        public abstract LinkResult Link(IConsole console, IStandardProject superProject, IStandardProject project, CompileResult assemblies, string outputDirectory);

        public abstract ProcessResult Size(IConsole console, IStandardProject project, LinkResult linkResult);

        public abstract string GetCompilerArguments(IStandardProject superProject, IStandardProject project, ISourceFile sourceFile); 

        public abstract string GetLinkerArguments(IStandardProject project);

        public abstract List<string> GetToolchainIncludes();

        public abstract bool SupportsFile(ISourceFile file);

        private object resultLock = new object();
        private int numTasks = 0;

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

        bool terminateBuild = false;

        private int GetFileCount(IStandardProject project)
        {
            int result = 0;

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

                result += project.SourceFiles.Where(sf=>SupportsFile(sf)).Count();
            }

            return result;
        }

        private int fileCount = 0;
        private int buildCount = 0;

        private void SetFileCount(IStandardProject project)
        {
            ClearBuildFlags(project);

            fileCount = GetFileCount(project);

            ClearBuildFlags(project);
        }

        public async Task<bool> Build(IConsole console, IProject project)
        {
            console.WriteLine("Starting Build...");

            bool result = true;
            terminateBuild = false;

            SetFileCount(project as IStandardProject);
            buildCount = 0;

            var compiledProjects = new List<CompileResult>();

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
                                Link(console, project as IStandardProject, compiledProject, linkedReferences);
                            }
                            else
                            {
                               // if (linkedReferences.Count > 0)
                                {
                                    linkedReferences.ObjectLocations = compiledProject.ObjectLocations;
                                    Link(console, project as IStandardProject, linkedReferences, linkedReferences);
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

            if (result)
            {                
                console.WriteLine("Build Successful");
            }
            else
            {
                console.WriteLine("Build Failed");
            }

            return result;
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

        private void Link(IConsole console, IStandardProject superProject, CompileResult compileResult, CompileResult linkResults)
        {
            var binDirectory = compileResult.Project.GetBinDirectory(superProject);

            if (!Directory.Exists(binDirectory))
            {
                Directory.CreateDirectory(binDirectory);
            }

            string outputLocation = binDirectory;

            string executable = Path.Combine(outputLocation, compileResult.Project.Name);

            if (compileResult.Project.Type == ProjectType.StaticLibrary)
            {
                executable = Path.Combine(outputLocation, "lib" + compileResult.Project.Name);
                executable += ".a";
            }
            else
            {
                executable += ".elf";
            }

            if (!Directory.Exists(outputLocation))
            {
                Directory.CreateDirectory(outputLocation);
            }

            console.OverWrite(string.Format("[LL]    [{0}]", compileResult.Project.Name));

            var linkResult = Link(console, superProject, compileResult.Project, compileResult, outputLocation);

            if (linkResult.ExitCode == 0)
            {                
                if (compileResult.Project.Type == ProjectType.StaticLibrary)
                {
                    linkResults.LibraryLocations.Add(executable);
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
        }

        private async Task CompileProject(IConsole console, IStandardProject superProject, IStandardProject project, List<CompileResult> results = null)
        {
            if (project.Type == ProjectType.Executable && superProject != project)
            {
                await Build(console, project);
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

                    bool doWork = false;

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
                        //var parallelResult = Parallel.ForEach(project.SourceFiles, (file) =>
                        int numLocalTasks = 0;

                        foreach (ISourceFile file in project.SourceFiles)
                        {
                            if (terminateBuild)
                            {
                                break;
                            }

                            if (SupportsFile(file))
                            {
                                var outputName = Path.GetFileNameWithoutExtension(file.Location) + ".o";
                                var dependencyFile = Path.Combine(objDirectory, Path.GetFileNameWithoutExtension(file.Location) + ".d");
                                var objectFile = Path.Combine(objDirectory, outputName);

                                bool dependencyChanged = false;

                                if (File.Exists(dependencyFile))
                                {
                                    List<string> dependencies = new List<string>();

                                    //lock(resultLock)
                                    {
                                        dependencies.AddRange(ProjectExtensions.GetDependencies(dependencyFile));

                                        foreach (var dependency in dependencies)
                                        {
                                            if (!File.Exists(dependency) || File.GetLastWriteTime(dependency) > File.GetLastWriteTime(objectFile))
                                            {
                                                dependencyChanged = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (dependencyChanged || !File.Exists(objectFile))
                                {
                                    while (numTasks >= Jobs)
                                    {
                                        Thread.Yield();
                                    }

                                    lock (resultLock)
                                    {
                                        numLocalTasks++;
                                        numTasks++;
                                        console.OverWrite(string.Format("[CC {0}/{1}]    [{2}]    {3}", ++buildCount, fileCount, project.Name, Path.GetFileName(file.Location)));
                                    }

                                    new Thread(() =>
                                    {
                                        var compileResult = Compile(console, superProject, project, file, objectFile);

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
                                            }

                                            numTasks--;
                                            numLocalTasks--;
                                        }
                                    }).Start();
                                }
                                else
                                {
                                    buildCount++;
                                    compileResults.ObjectLocations.Add(objectFile);
                                }
                            }
                        }
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

            string outputDirectory = project.GetObjectDirectory(superProject);

            bool hasCleaned = false;

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

        public async Task Clean(IConsole console, IProject project)
        {
            await Task.Factory.StartNew(async () =>
            {
                console.WriteLine("Starting Clean...");

                await CleanAll(console, project as IStandardProject, project as IStandardProject);

                console.WriteLine("Clean Completed.");
            });
        }        

        public IList<string> Includes
        {
            get
            {
                return GetToolchainIncludes();
            }
        }

        public abstract IList<TabItem> GetConfigurationPages(IProject project);

        public abstract bool CanHandle(IProject project);

        public abstract void ProvisionSettings(IProject project);

        public abstract UserControl GetSettingsControl(IProject project);

        public string Name { get { return GetType().ToString(); } }

        public abstract Version Version { get; }

        public abstract string Description { get; }
    }
}
