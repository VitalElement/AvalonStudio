namespace AvalonStudio.Models.Tools.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using AvalonStudio.Models.Solutions;

    public static class ProjectExtensions
    {
        public static string GetOutputDirectory(this Project project, Project superProject)
        {
            string outputDirectory = Path.Combine(project.CurrentDirectory, "obj");

            if (project != superProject)
            {
                outputDirectory = Path.Combine(outputDirectory, superProject.Title);
            }

            return outputDirectory;
        }

        public static List<string> GetDependencies(string dependencyFile)
        {
            var result = new List<string>();

            StreamReader sr = new StreamReader(dependencyFile);

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();

                if (!string.IsNullOrEmpty(line))
                {
                    if (!line.EndsWith(":") && !line.EndsWith(": \\"))
                    {
                        result.Add(line.Replace(" \\", string.Empty).Trim());
                    }
                }
            }

            sr.Close();

            return result;
        }

        public static bool ToBuild(this Project project, StandardToolChain toolchain, Project superProject)
        {
            bool result = false;

            var outputDirectory = project.GetOutputDirectory(superProject);

            project.VisitAllFiles((file) =>
            {
                if (Path.GetExtension(file.Location) == ".c" || Path.GetExtension(file.Location) == ".cpp")
                {
                    var outputName = Path.GetFileNameWithoutExtension(file.Location) + ".o";
                    var objectFile = Path.Combine(outputDirectory, outputName);
                    var dependencyFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(file.Location) + ".d");

                    if (!File.Exists(objectFile))
                    {
                        result = true;
                        return true;
                    }

                    if (File.Exists(dependencyFile))
                    {
                        var dependencies = ProjectExtensions.GetDependencies(dependencyFile);

                        foreach (var dependency in dependencies)
                        {
                            DateTime lastWriteTime = File.GetLastWriteTime(dependency);
                            if (!File.Exists(dependency) || File.GetLastWriteTime(dependency) > File.GetLastWriteTime(objectFile))
                            {
                                result = true;
                                return true;
                            }
                        }
                    }
                }

                return false;
            });

            return result;
        }
    }

    public class StringConsole : IConsole
    {
        public StringConsole()
        {
            Value = string.Empty;
        }

        public string Value { get; set; }

        public void Clear()
        {
            Value = string.Empty;
        }

        public void Write(char data)
        {
            Value += data;
        }

        public void Write(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                Value += data;
            }
        }

        public void WriteLine()
        {
            Value += Environment.NewLine;
        }

        public void WriteLine(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                Value += data + Environment.NewLine;
            }
        }
    }

    public abstract class StandardToolChain : ToolChain
    {
        public abstract void Compile(IConsole console, Project superProject, Project project, ProjectFile file, string outputFile, CompileResult result);

        public abstract LinkResult Link(IConsole console, Project superProject, Project project, CompileResult assemblies, string outputDirectory);

        public abstract ProcessResult Size(IConsole console, Project project, LinkResult linkResult);

        public abstract string GetCompilerArguments(Project project, FileType language);

        public abstract string GetLinkerArguments(Project project);

        public override async Task<bool> Build(IConsole console, Project project, CancellationTokenSource cancellationSource)
        {
            bool result = false;

            console.Clear();
            console.WriteLine("Starting Build...");

            if (project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation == null)
            {
                console.WriteLine("Tool chain path has not been configured.");

                result = false;
            }
            else
            {
                if (project.SelectedConfiguration.IsLibrary)
                {
                    result = (await BuildLibrary(console, project, project, cancellationSource)).ExitCode == 0;
                }
                else
                {
                    result = (await BuildExecutable(console, project, cancellationSource)).ExitCode == 0;
                }
            }

            console.WriteLine();

            if (result)
            {
                console.WriteLine("Build Completed Successfully.");
            }
            else
            {
                console.WriteLine("Build Failed.");
            }

            return result;
        }

        private async Task<CompileResult> BuildExecutable(IConsole console, Project project, CancellationTokenSource cancellationSource)
        {
            var result = new CompileResult();

            var compileResults = new CompileResult();

            foreach (var reference in project.LoadedReferences)
            {
                if (reference.SelectedConfiguration.IsLibrary)
                {
                    var referenceResult = await BuildLibrary(console, project, reference, cancellationSource);

                    if (referenceResult.ExitCode == 0)
                    {
                        compileResults.LibraryLocations.AddRange(referenceResult.LibraryLocations);
                        compileResults.NumberOfObjectsCompiled += referenceResult.NumberOfObjectsCompiled;
                    }
                    else
                    {
                        result.ExitCode = -1;
                        return result;
                    }
                }
                else
                {
                    var subResult = await BuildExecutable(console, reference, cancellationSource);

                    if (subResult.ExitCode == 0)
                    {
                        foreach (var executable in subResult.ExecutableLocations)
                        {
                            string outputDirectory = Path.Combine(project.CurrentDirectory, "bin");

                            string destination = Path.Combine(outputDirectory, Path.GetFileName(executable));

                            if (!Directory.Exists(outputDirectory))
                            {
                                Directory.CreateDirectory(outputDirectory);
                            }

                            File.Copy(executable, destination, true);
                        }
                    }
                    else
                    {
                        result.ExitCode = -1;
                        return result;
                    }
                }
            }

            //bool hasBuiltSomething = false;

            //if (project.ToBuild(this, project))
            //{
            //    hasBuiltSomething = true;
            //    console.WriteLine(string.Format("[BB] - Building Executable - {0}", project.Title));
            //}

            var compilationResult = await Compile(console, project, project, cancellationSource);

            //if (hasBuiltSomething)
            //{
            //    console.WriteLine();
            //}

            compilationResult.NumberOfObjectsCompiled += compileResults.NumberOfObjectsCompiled;

            if (compilationResult.ExitCode == 0)
            {
                if (compilationResult.Count > 0)
                {
                    compilationResult.ObjectLocations.AddRange(compileResults.ObjectLocations);
                    compilationResult.LibraryLocations.AddRange(compileResults.LibraryLocations);

                    result = Link(console, project, project, compilationResult, cancellationSource);
                }

                return result;
            }
            else
            {
                result.ExitCode = -1;
                return result;
            }
        }

        private CompileResult Link(IConsole console, Project superProject, Project project, CompileResult compilationResults, CancellationTokenSource cancellationSource)
        {
            var result = new CompileResult();

            string outputLocation = string.Empty;

            if (project.SelectedConfiguration.IsLibrary)
            {
                outputLocation = Path.Combine(superProject.CurrentDirectory, "obj");
            }
            else
            {
                outputLocation = Path.Combine(superProject.CurrentDirectory, "bin");
            }

            if (!Directory.Exists(outputLocation))
            {
                Directory.CreateDirectory(outputLocation);
            }

            //Switch toolchains to the super projects.
            var superProjectToolchain = superProject.SelectedConfiguration.ToolChain as StandardToolChain;

            if (!File.Exists(superProject.Executable) || compilationResults.NumberOfObjectsCompiled > 0)
            {
                console.WriteLine(string.Format("[LL]    [{0}]", project.Title));
                
                var linkResults = superProjectToolchain.Link(console, superProject, project, compilationResults, outputLocation);

                if (linkResults.ExitCode == 0)
                {
                    project.Executable = linkResults.Executable;

                    if (project.SelectedConfiguration.IsLibrary)
                    {
                        result.LibraryLocations.Add(project.Executable);
                    }
                    else
                    {
                        Size(console, project, linkResults);
                        result.ExecutableLocations.Add(project.Executable);
                    }

                    result.NumberOfObjectsCompiled += compilationResults.NumberOfObjectsCompiled;
                }
                else
                {
                    project.Executable = null;
                    result.ExitCode = -1;
                }

                console.WriteLine();
            }
            else
            {
                if (project.SelectedConfiguration.IsLibrary)
                {
                    result.LibraryLocations.Add(project.Executable);
                }
                else
                {
                    result.ExecutableLocations.Add(project.Executable);
                }

                if (superProject == project)
                {
                    Size(console, project, new LinkResult() { Executable = superProject.Executable });
                    console.WriteLine();
                }
            }

            return result;
        }

        private async Task<CompileResult> BuildLibrary(IConsole console, Project superProject, Project project, CancellationTokenSource cancellationSource)
        {
            var result = new CompileResult();

            CompileResult referenceResults = new CompileResult();

            foreach (var reference in project.LoadedReferences)
            {
                if (reference.SelectedConfiguration.IsLibrary)
                {
                    var refResult = await BuildReference(console, superProject, reference, cancellationSource);

                    if (refResult.ExitCode == 0)
                    {
                        foreach (var obj in refResult.ObjectLocations)
                        {
                            referenceResults.ObjectLocations.Add(obj);
                        }

                        referenceResults.NumberOfObjectsCompiled += refResult.NumberOfObjectsCompiled;
                    }
                    else
                    {
                        result.ExitCode = -1;

                        return result;
                    }
                }
                else
                {
                    var subResult = await BuildExecutable(console, reference, cancellationSource);

                    if (subResult.ExitCode == 0)
                    {
                        foreach (var executable in subResult.ExecutableLocations)
                        {
                            string outputDirectory = Path.Combine(project.CurrentDirectory, "bin");

                            string destination = Path.Combine(outputDirectory, Path.GetFileName(executable));

                            if (!Directory.Exists(outputDirectory))
                            {
                                Directory.CreateDirectory(outputDirectory);
                            }

                            File.Copy(executable, destination, true);
                        }
                    }
                    else
                    {
                        result.ExitCode = -1;
                        return result;
                    }
                }
            }

           // bool hasBuiltSomething = false;

            //if (project.ToBuild(this, superProject))
            //{
            //    hasBuiltSomething = true;
            //    console.WriteLine(string.Format("[BB] - Building Library - {0}", project.Title));
            //}

            var compilationResult = await Compile(console, superProject, project, cancellationSource);
            compilationResult.NumberOfObjectsCompiled += referenceResults.NumberOfObjectsCompiled;

            //if (hasBuiltSomething)
            //{
            //    console.WriteLine();
            //}

            if (compilationResult.ExitCode == 0)
            {
                compilationResult.ObjectLocations.AddRange(referenceResults.ObjectLocations);

                if (compilationResult.Count > 0)
                {
                    result.NumberOfObjectsCompiled += compilationResult.NumberOfObjectsCompiled;
                    result = Link(console, superProject, project, compilationResult, cancellationSource);
                }

                return result;
            }
            else
            {
                return compilationResult;
            }
        }


        private async Task<CompileResult> Compile(IConsole console, Project superProject, Project project, CancellationTokenSource cancellationSource)
        {
            CompileResult result = new CompileResult();

            if (project.Children.Count == 0)
            {
                return result;
            }

            await Task.Factory.StartNew(() =>
           {
               var outputDirectory = project.GetOutputDirectory(superProject);

               if (!Directory.Exists(outputDirectory))
               {
                   Directory.CreateDirectory(outputDirectory);
               }

               Semaphore compileThread = new Semaphore(16, 16);
               int compileJobs = 0;
               object compileJobsLock = new object();

               var superProjectToolchain = superProject.SelectedConfiguration.ToolChain as StandardToolChain;

               project.VisitAllFiles((file) =>
               {
                   if (Path.GetExtension(file.Location) == ".c" || Path.GetExtension(file.Location) == ".cpp")
                   {
                       var outputName = Path.GetFileNameWithoutExtension(file.Location) + ".o";
                       var dependencyFile = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(file.Location) + ".d");
                       var objectFile = Path.Combine(outputDirectory, outputName);

                       bool dependencyChanged = false;
                       object resultLock = new object();

                       if (File.Exists(dependencyFile))
                       {
                           var dependencies = ProjectExtensions.GetDependencies(dependencyFile);

                           foreach (var dependency in dependencies)
                           {
                               if (!File.Exists(dependency) || File.GetLastWriteTime(dependency) > File.GetLastWriteTime(objectFile))
                               {
                                   dependencyChanged = true;
                                   break;
                               }
                           }
                       }

                       if (dependencyChanged || !File.Exists(objectFile))
                       {
                           compileThread.WaitOne();

                           lock (compileJobsLock)
                           {
                               compileJobs++;
                           }

                           console.WriteLine(string.Format("[CC]    [{0}]    {1}", project.Title, Path.GetFileName(file.Location)));                           

                           new Thread(() =>
                           {
                               superProjectToolchain.Compile(console, superProject, project, file, objectFile, result);
                               compileThread.Release(1);

                               lock(resultLock)
                               {
                                   if (result.ExitCode == 0 && File.Exists(objectFile))
                                   {
                                       result.ObjectLocations.Add(objectFile);
                                       result.NumberOfObjectsCompiled++;
                                   }
                                   else
                                   {
                                       console.WriteLine("Compilation failed.");
                                   }
                               }

                               lock(compileJobsLock)
                               {
                                   compileJobs--;
                               }

                           }).Start();


                       }
                       else
                       {
                           result.ObjectLocations.Add(objectFile);
                       }
                   }
                   else
                   {
                       return false;
                   }

                   return false;
               });


               while (compileJobs != 0)
               {
                   Thread.Sleep(10);
               };

               //project.SaveChanges();
           }, cancellationSource.Token);

            return result;
        }

        private async Task<CompileResult> BuildReference(IConsole console, Project superProject, Project project, CancellationTokenSource cancellationSource)
        {
            var compileResults = new CompileResult();
            //bool hasBuiltSomething = false;

            foreach (var reference in project.LoadedReferences)
            {
                var result = await BuildReference(console, superProject, reference, cancellationSource);

                if (result.ExitCode == 0)
                {
                    compileResults.NumberOfObjectsCompiled += result.NumberOfObjectsCompiled;
                    compileResults.ObjectLocations.AddRange(result.ObjectLocations);
                }
                else
                {
                    compileResults.ExitCode = -1;
                    return compileResults;
                }
            }

            //if (project.ToBuild(this, superProject))
            //{
            //    console.WriteLine(string.Format("[BB] - Building Referenced Project - {0}", project.Title));
            //    hasBuiltSomething = true;
            //}

            var superResults = await Compile(console, superProject, project, cancellationSource);

            //if (hasBuiltSomething)
            //{
            //    console.WriteLine();
            //}

            if (superResults.ExitCode == 0)
            {
                compileResults.NumberOfObjectsCompiled += superResults.NumberOfObjectsCompiled;
                compileResults.ObjectLocations.AddRange(superResults.ObjectLocations);
                return compileResults;
            }
            else
            {
                superResults.ExitCode = -1;
                return superResults;
            }
        }

        private async Task CleanAll(IConsole console, Project project, CancellationTokenSource cancellationSource)
        {
            foreach (var reference in project.LoadedReferences)
            {
                await CleanAll(console, reference, cancellationSource);
            }

            string outputDirectory = Path.Combine(project.CurrentDirectory, "obj");

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

            outputDirectory = Path.Combine(project.CurrentDirectory, "bin");

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
                console.WriteLine(string.Format("[BB] - Cleaning Project - {0}", project.Title));
            }
        }

        public override async Task Clean(IConsole console, Project project, CancellationTokenSource cancellationSource)
        {
            console.Clear();
            console.WriteLine("Starting Clean...");
            await CleanAll(console, project, cancellationSource);
            console.WriteLine("Clean Completed.");
        }
    }
}
