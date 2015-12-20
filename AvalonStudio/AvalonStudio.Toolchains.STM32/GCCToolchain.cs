namespace AvalonStudio.Toolchains.STM32
{
    using AvalonStudio.Toolchains.Standard;
    using Projects;
    using Projects.Standard;
    using System.Diagnostics;
    using System.IO;
    using Utils;
    using System;
    using System.Collections.Generic;

    public class GccToolChain : StandardToolChain
    {
        public GccToolChain(ToolchainSettings settings) : base(settings)
        {
        }

        public override CompileResult Compile(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string outputFile)
        {
            CompileResult result = new CompileResult();

            var startInfo = new ProcessStartInfo();

            if (file.Language == Language.Cpp)
            {
                startInfo.FileName = Path.Combine(Settings.ToolChainLocation, "arm-none-eabi-g++.exe");
            }
            else
            {
                startInfo.FileName = Path.Combine(Settings.ToolChainLocation, "arm-none-eabi-gcc.exe");
            }


            startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

            if (!File.Exists(startInfo.FileName))
            {
                result.ExitCode = -1;
                console.WriteLine("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
            }
            else
            {
                string fileArguments = string.Empty;

                if (file.Language == Language.Cpp)
                {
                    fileArguments = "-x c++ -std=c++14 -fno-use-cxa-atexit";
                }

                startInfo.Arguments = string.Format("{0} {1} {2} -o{3} -MMD -MP", GetCompilerArguments(superProject, project, file), fileArguments, file.Location, outputFile);

                // Hide console window
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;

                //console.WriteLine (Path.GetFileNameWithoutExtension(startInfo.FileName) + " " + startInfo.Arguments);

                using (var process = Process.Start(startInfo))
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        console.WriteLine(e.Data);
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            console.WriteLine();
                            console.WriteLine(e.Data);
                        }
                    };

                    process.BeginOutputReadLine();

                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    result.ExitCode = process.ExitCode;
                }
            }

            return result;
        }

        public override LinkResult Link(IConsole console, IStandardProject superProject, IStandardProject project, CompileResult assemblies, string outputDirectory)
        {
            LinkResult result = new LinkResult();

            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(Settings.ToolChainLocation, "arm-none-eabi-gcc.exe");

            if (project.Type == ProjectType.StaticLibrary)
            {
                startInfo.FileName = Path.Combine(Settings.ToolChainLocation, "arm-none-eabi-ar.exe");
            }

            startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

            if (!File.Exists(startInfo.FileName))
            {
                result.ExitCode = -1;
                console.WriteLine("Unable to find linker executable (" + startInfo.FileName + ") Check project compiler settings.");
                return result;
            }

            // GenerateLinkerScript(project);

            string objectArguments = string.Empty;
            foreach (string obj in assemblies.ObjectLocations)
            {
                objectArguments += obj + " ";
            }

            string libs = string.Empty;
            foreach (string lib in assemblies.LibraryLocations)
            {
                libs += lib + " ";
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string outputName = Path.GetFileNameWithoutExtension(project.Location) + ".elf";

            if (project.Type == ProjectType.StaticLibrary)
            {
                outputName = "lib" + Path.GetFileNameWithoutExtension(project.Name) + ".a";
            }

            var executable = Path.Combine(outputDirectory, outputName);

            string linkedLibraries = string.Empty;

            //foreach (var libraryPath in project.SelectedConfiguration.LinkedLibraries)
            //{
            //    string relativePath = Path.GetDirectoryName(libraryPath);

            //    string libName = Path.GetFileNameWithoutExtension(libraryPath).Substring(3);

            //    linkedLibraries += string.Format(" -L\"{0}\" -l{1}", relativePath, libName);
            //}

            foreach (var lib in project.BuiltinLibraries)
            {
                linkedLibraries += string.Format("-l{0} ", lib);
            }


            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;

            startInfo.Arguments = string.Format("{0} -o{1} {2} -Wl,--start-group {3} {4} -Wl,--end-group", GetLinkerArguments(project), executable, objectArguments, linkedLibraries, libs);

            if (project.Type == ProjectType.StaticLibrary)
            {
                startInfo.Arguments = string.Format("rvs {0} {1}", executable, objectArguments);
            }

            //console.WriteLine(Path.GetFileNameWithoutExtension(startInfo.FileName) + " " + startInfo.Arguments);
            //console.WriteLine ("[LL] - " + startInfo.Arguments);

            using (var process = Process.Start(startInfo))
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    //console.WriteLine(e.Data);
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null && !e.Data.Contains("creating"))
                    {
                        console.WriteLine(e.Data);
                    }
                };

                process.BeginOutputReadLine();

                process.BeginErrorReadLine();

                process.WaitForExit();

                result.ExitCode = process.ExitCode;

                if (result.ExitCode == 0)
                {
                    result.Executable = executable;
                }
            }

            return result;
        }

        public override ProcessResult Size(IConsole console, IStandardProject project, LinkResult linkResult)
        {
            ProcessResult result = new ProcessResult();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine(Settings.ToolChainLocation, "arm-none-eabi-size.exe");

            if (!File.Exists(startInfo.FileName))
            {
                console.WriteLine("Unable to find tool (" + startInfo.FileName + ") check project compiler settings.");
                result.ExitCode = -1;
                return result;
            }

            startInfo.Arguments = string.Format("{0}", linkResult.Executable);

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;


            using (var process = Process.Start(startInfo))
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    console.WriteLine(e.Data);
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    console.WriteLine(e.Data);
                };

                process.BeginOutputReadLine();

                process.BeginErrorReadLine();

                process.WaitForExit();

                result.ExitCode = process.ExitCode;
            }

            return result;
        }

        #region Settings
        public enum GCCOptimizationLevel
        {
            //[Description ("-O0")]
            Off,
            //[Description ("-O1")]
            Level1,
            //[Description ("-O2")]
            Level2,
            //[Description ("-O3")]
            Level3
        }

        public enum GCCOptimizationPreference
        {
            //[Description("")]
            None,
            //[Description ("-Os")]
            Size,
            //[Description ("-Ofast")]
            Speed
        }

        public GCCOptimizationLevel Optimization { get; set; }
        public GCCOptimizationPreference OptimizationPriority { get; set; }

        public string CompilerCustomArguments { get; set; }

        public string LinkerCustomArguments { get; set; }

        public string LinkerScript { get; set; }
        #endregion

        public override string GetLinkerArguments(IStandardProject project)
        {
            string result = string.Empty;

            foreach (var arg in project.ToolChainArguments)
            {
                result += string.Format(" {0}", arg);
            }

            foreach (var arg in project.LinkerArguments)
            {
                result += string.Format(" {0}", arg);
            }

            result += string.Format(" -L{0} -Wl,-T\"{1}\"", project.CurrentDirectory, project.LinkerScript);

            return result;
        }

        public override string GetCompilerArguments(IStandardProject superProject, IStandardProject project, ISourceFile file)
        {
            string result = string.Empty;

            result += "-Wall -c -g ";

            // toolchain includes

            // Referenced includes
            var referencedIncludes = project.GetReferencedIncludes();

            foreach (var include in referencedIncludes)
            {
                result += string.Format("-I\"{0}\" ", Path.Combine(project.CurrentDirectory, include));
            }

            // global includes
            var globalIncludes = superProject.GetGlobalIncludes();

            foreach (var include in globalIncludes)
            {
                result += string.Format("-I\"{0}\" ", include);
            }

            // public includes
            foreach (var include in project.PublicIncludes)
            {
                result += string.Format("-I\"{0}\" ", Path.Combine(project.CurrentDirectory, include));
            }

            // includes
            foreach (var include in project.Includes)
            {
                result += string.Format("-I\"{0}\" ", Path.Combine(project.CurrentDirectory, include));
            }


            foreach (var define in superProject.Defines)
            {
                result += string.Format("-D{0} ", define);
            }

            foreach (var arg in superProject.ToolChainArguments)
            {
                result += string.Format(" {0}", arg);
            }

            foreach (var arg in superProject.CompilerArguments)
            {
                result += string.Format(" {0}", arg);
            }

            switch (file.Language)
            {
                case Language.C:
                    {
                        foreach (var arg in superProject.CCompilerArguments)
                        {
                            result += string.Format(" {0}", arg);
                        }
                    }
                    break;

                case Language.Cpp:
                    {
                        foreach (var arg in superProject.CppCompilerArguments)
                        {
                            result += string.Format(" {0}", arg);
                        }
                    }
                    break;
            }

            return result;
        }

        public override List<string> GetToolchainIncludes()
        {
            return new List<string>()
            {
                @"arm-none-eabi\include",
                @"arm-none-eabi\include\c++\4.9.3",
                @"arm-none-eabi\include\c++\4.9.3\arm-none-eabi\thumb",
                @"lib\gcc\arm-none-eabi\4.9.3\include"
            };
        }

        //public void GenerateLinkerScript(Project project)
        //{
        //    var template = new ArmGCCLinkTemplate(project.SelectedConfiguration);

        //    string linkerScript = GetLinkerScriptLocation(project);

        //    if (File.Exists(linkerScript))
        //    {
        //        File.Delete(linkerScript);
        //    }

        //    var sw = File.CreateText(linkerScript);

        //    sw.Write(template.TransformText());

        //    sw.Close();
        //}

        //private string GetLinkerScriptLocation(Project project)
        //{
        //    return Path.Combine(project.CurrentDirectory, "link.ld");
        //}

        public override string GDBExecutable
        {
            get
            {
                string binDirectory = Path.Combine(Settings.ToolChainLocation, "bin");
                return Path.Combine(binDirectory, "arm-none-eabi-gdb.exe");
            }
        }
    }
}

