﻿namespace AvalonStudio.Toolchains
{
    using AvalonStudio.Utils;
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class StandardToolChain : ToolChain
    {
        public StandardToolChain(ToolchainSettings settings)
        {
            this.Settings = settings;
            this.Jobs = 1;
        }

        public int Jobs { get; set; }

        protected ToolchainSettings Settings { get; private set; }

        public abstract CompileResult Compile(IConsole console, Project superProject, Project project, SourceFile file, string outputFile);

        public abstract LinkResult Link(IConsole console, Project superProject, Project project, CompileResult assemblies, string outputDirectory);

        public abstract ProcessResult Size(IConsole console, Project project, LinkResult linkResult);

        public abstract string GetCompilerArguments(Project superProject, Project project);

        public abstract string GetLinkerArguments(Project project);

        private object resultLock = new object();
        private int numTasks = 0;

        private void ClearBuildFlags(Project project)
        {
            foreach (var reference in project.References)
            {
                var loadedReference = project.GetReference(reference);

                ClearBuildFlags(loadedReference);
            }

            project.IsBuilding = false;
        }

        bool terminateBuild = false;

        private int GetFileCount (Project project)
        {
            int result = 0;

            foreach (var reference in project.References)
            {
                var loadedReference = project.GetReference(reference);

                result += GetFileCount(loadedReference);                
            }

            if(!project.IsBuilding)
            {
                project.IsBuilding = true;

                result += project.SourceFiles.Count;
            }

            return result;
        }

        private int fileCount = 0;
        private int buildCount = 0;

        private void SetFileCount(Project project)
        {
            ClearBuildFlags(project);

            fileCount = GetFileCount(project);

            ClearBuildFlags(project);
        }

        public override async Task<bool> Build(IConsole console, Project project)
        {
            console.WriteLine("Starting Build...");

            bool result = true;
            terminateBuild = false;

            SetFileCount(project);
            buildCount = 0;
            
            var compiledProjects = new List<CompileResult>();

            if (!terminateBuild)
            {
                await CompileProject(console, project, project, compiledProjects);

                if (!terminateBuild)
                {
                    await WaitForCompileJobs();
                    
                    foreach (var compiledReference in compiledProjects)
                    {
                        result = compiledReference.ExitCode == 0;

                        if(!result)
                        {
                            break;
                        }
                    }

                    if (result)
                    {
                        var linkedReferences = new CompileResult();
                        linkedReferences.Project = project;

                        foreach (var compiledProject in compiledProjects)
                        {
                            if (compiledProject.Project != project)
                            {
                                Link(console, project, compiledProject, linkedReferences);
                            }
                            else
                            {
                                if (linkedReferences.Count > 0)
                                {
                                    linkedReferences.ObjectLocations = compiledProject.ObjectLocations;
                                    Link(console, project, linkedReferences, linkedReferences);
                                }
                            }

                            if(linkedReferences.ExitCode != 0)
                            {
                                result = false;
                                break;
                            }
                        }
                    }

                    ClearBuildFlags(project);
                }
            }

            console.WriteLine();

            if(result)
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

        private void Link(IConsole console, Project superProject, CompileResult compileResult, CompileResult linkResults)
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
                    console.WriteLine();
                    Size(console, compileResult.Project, linkResult);
                    linkResults.ExecutableLocations.Add(executable);
                }
            }
            else if(linkResults.ExitCode == 0)
            {
                linkResults.ExitCode = linkResult.ExitCode;
            }
        }

        private async Task CompileProject(IConsole console, Project superProject, Project project, List<CompileResult> results = null)
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
                        var loadedReference = project.GetReference(reference);

                        await CompileProject(console, superProject, loadedReference, results);
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

                        foreach (var file in project.SourceFiles)
                        {
                            if (terminateBuild)
                            {
                                break;
                            }

                            if (Path.GetExtension(file.Location) == ".c" || Path.GetExtension(file.Location) == ".cpp")
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

                                    new Thread(new ThreadStart(() =>
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
                                    })).Start();
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

        private async Task CleanAll(IConsole console, Project superProject, Project project)
        {
            foreach (var reference in project.References)
            {               
                var loadedReference = project.GetReference(reference);

                if (loadedReference.Type == ProjectType.Executable)
                {
                    await CleanAll(console, loadedReference, loadedReference);
                }
                else
                {
                    await CleanAll(console, superProject, loadedReference);
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

        public override async Task Clean(IConsole console, Project project)
        {
            await Task.Factory.StartNew(async () =>
            {
                console.WriteLine("Starting Clean...");

                await CleanAll(console, project, project);

                console.WriteLine("Clean Completed.");
            });
        }
    }
}
