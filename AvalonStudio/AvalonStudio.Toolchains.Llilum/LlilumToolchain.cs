namespace AvalonStudio.Toolchains.Llilum
{
    using AvalonStudio.Toolchains.Standard;
    using AvalonStudio.Projects.Standard;
    using Projects;
    using Utils;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Perspex.Controls;

    public class LlilumToolchain : StandardToolChain
    {
        public LlilumToolchain () 
        {

        }        

        public override Version Version
        {
            get
            {
                return new Version(1, 0, 0, 0);
            }
        }

        public override string Description
        {
            get
            {
                return "Experimental toolchain for Llilum.";
            }
        }

        private string BaseDirectory
        {
            get
            {
                return Path.Combine(Platform.Platform.AppDataDirectory, "AvalonStudio.Toolchains.Lillum");
            }
        }

        private void CompileCS(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string outputFile)
        {
            var startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(BaseDirectory, "Roslyn", "csc.exe");

            if (!File.Exists(startInfo.FileName))
            {
                console.WriteLine("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
            }
            else
            {
                startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

                startInfo.Arguments = string.Format("{0} /out:{1} {2}", GetCSCCompilerArguments(superProject, project, file), outputFile, file.Location);

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
                }
            }
        }

        private void TransformMSIL(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string outputFile)
        {
            var startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(BaseDirectory, "Llilum\\ZeligBuild\\Host\\bin\\Debug", "Microsoft.Zelig.Compiler.exe");

            if (!File.Exists(startInfo.FileName))
            {
                console.WriteLine("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
            }
            else
            {
                //startInfo.WorkingDirectory = Path.Combine(BaseDirectory, "Llilum\\ZeligBuild\\Host\\bin\\Debug");

                startInfo.Arguments = string.Format("{0} -OutputName {1} {2}", GetZeligCompilerArguments(superProject, project, file), outputFile, outputFile);

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
                }
            }
        }

        private void CompileLLVMIR(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string llvmBinary, string outputFile)
        {
            var startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(BaseDirectory, "LLVM", "llc.exe");

            if (!File.Exists(startInfo.FileName))
            {
                console.WriteLine("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
            }
            else
            {
                startInfo.WorkingDirectory = Path.Combine(BaseDirectory, "Llilum\\ZeligBuild\\Host\\bin\\Debug");

                startInfo.Arguments = string.Format("-O0 -code-model=small -data-sections -relocation-model=pic -march=thumb -mcpu=cortex-m4 -filetype=obj -mtriple=Thumb-NoSubArch-UnknownVendor-UnknownOS-GNUEABI-ELF -o={0} {1}", outputFile, llvmBinary);

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
                }
            }
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
                result += string.Format("-I\"{0}\" ", Path.Combine(project.CurrentDirectory, include.Value));
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

        public override CompileResult Compile(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string outputFile)
        {
            CompileResult result = new CompileResult();

            if (Path.GetExtension(file.File) == ".cs")
            {
                CompileCS(console, superProject, project, file, outputFile);

                TransformMSIL(console, superProject, project, file, outputFile);

                CompileLLVMIR(console, superProject, project, file, outputFile + ".bc", outputFile);


            }
            else if (Path.GetExtension (file.File) == ".cpp" || Path.GetExtension(file.File) == ".c")
            {
                var startInfo = new ProcessStartInfo();

                if (file.Language == Language.Cpp)
                {
                    startInfo.FileName = Path.Combine(BaseDirectory, "GCC\\bin", "arm-none-eabi-g++.exe");
                }
                else
                {
                    startInfo.FileName = Path.Combine(BaseDirectory, "GCC\\bin", "arm-none-eabi-gcc.exe");
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
            }

            return result;
        }

        public string GetCSCCompilerArguments(IStandardProject superProject, IStandardProject project, ISourceFile sourceFile)
        {
            return string.Format("/unsafe");
        }

        public string GetZeligCompilerArguments(IStandardProject superProject, IStandardProject project, ISourceFile sourceFile)
        {
            string result = string.Empty;

            result += string.Format("-HostAssemblyDir {0} ", Path.Combine(BaseDirectory, "Llilum\\ZeligBuild\\Host\\bin\\Debug"));
            result += string.Format("-DeviceAssemblyDir {0} ", Path.Combine(BaseDirectory, "Llilum\\ZeligBuild\\Target\\bin\\Debug"));
            result += string.Format("-CompilationSetupPath {0} ", Path.Combine(BaseDirectory, "Llilum\\ZeligBuild\\Host\\bin\\Debug\\Microsoft.Llilum.BoardConfigurations.STM32F411.dll"));
            result += string.Format("-CompilationSetup Microsoft.Llilum.BoardConfigurations.STM32F411MBEDHostedCompilationSetup ");
            result += "-Reference Microsoft.CortexM4OnMBED ";
            result += "-Reference Microsoft.CortexM4OnCMSISCore ";
            result += "-Reference DeviceModels.ModelForCortexM4 ";
            result += "-Reference STM32F411 ";
            result += "-Reference Microsoft.Zelig.LlilumCMSIS-RTOS ";
            result += "-Reference Microsoft.Zelig.Runtime ";

            result += "-CompilationPhaseDisabled InitializeReferenceCountingGarbageCollection ";
            result += "-CompilationPhaseDisabled EnableStrictReferenceCountingGarbageCollection ";
            result += "-CompilationPhaseDisabled ResourceManagerOptimizations ";

            result += "-CompilationPhaseDisabled PrepareExternalMethods ";
            result += "-CompilationPhaseDisabled MidLevelToLowLevelConversion ";
            result += "-CompilationPhaseDisabled ConvertUnsupportedOperatorsToMethodCalls ";
            result += "-CompilationPhaseDisabled ExpandAggregateTypes ";
            result += "-CompilationPhaseDisabled SplitComplexOperators ";
            result += "-CompilationPhaseDisabled FuseOperators ";

            result += "-CompilationPhaseDisabled ConvertToSSA ";
            result += "-CompilationPhaseDisabled PrepareForRegisterAllocation ";
            result += "-CompilationPhaseDisabled CollectRegisterAllocationConstraints ";
            result += "-CompilationPhaseDisabled AllocateRegisters ";
            result += "-DumpIR ";
            result += "-DumpLLVMIR ";
            result += "-MaxProcs 8 ";
            result += "-NoSDK ";

            result += string.Format("-LlvmBinPath {0} ", Path.Combine(BaseDirectory, "LLVM"));

            return result;
        }

        public override LinkResult Link(IConsole console, IStandardProject superProject, IStandardProject project, CompileResult assemblies, string outputDirectory)
        {
            LinkResult result = new LinkResult();

            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(BaseDirectory, "GCC\\bin", "arm-none-eabi-gcc.exe");

            if (project.Type == ProjectType.StaticLibrary)
            {
                startInfo.FileName = Path.Combine(BaseDirectory, "GCC\\bin",  "arm-none-eabi-ar.exe");
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

            foreach (var libraryPath in project.StaticLibraries)
            {
                string relativePath = project.CurrentDirectory;

                string libName = Path.GetFileNameWithoutExtension(libraryPath).Substring(3);

                linkedLibraries += string.Format(" -L\"{0}\" -l{1} ", relativePath, libName);
            }

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
            startInfo.FileName = Path.Combine(BaseDirectory, "GCC\\bin", "arm-none-eabi-size.exe");

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

            result += string.Format(" -L{0} -Wl,-T\"{1}\"", project.CurrentDirectory, "link.ld");

            return result;
        }

        public override List<string> GetToolchainIncludes()
        {
            //throw new NotImplementedException();

            return new List<string>()
            {
                "c:\\VEStudio\\AppData\\Repos\\GCCToolChain\\arm-none-eabi\\include",
                "c:\\VEStudio\\AppData\\Repos\\GCCToolChain\\arm-none-eabi\\include\\c++\\4.9.3",
                "c:\\VEStudio\\AppData\\Repos\\GCCToolChain\\arm-none-eabi\\c++\\4.9.3\\arm-none-eabi\\thumb",
                "c:\\VEStudio\\AppData\\Repos\\GCCToolChain\\lib\\gcc\\arm-none-eabi\\4.9.3\\include"
            };
        }


        public override bool SupportsFile(ISourceFile file)
        {
            bool result = false;

            var extension = Path.GetExtension(file.Location);

            switch (extension)
            {
                case ".cs":
                case ".cpp":
                case ".c":
                    result = true;
                    break;
            }            

            return result;
        }

        public override IList<TabItem> GetConfigurationPages(IProject project)
        {
            throw new NotImplementedException();
        }

        public override bool CanHandle(IProject project)
        {
            return false;
        }

        public override void ProvisionSettings(IProject project)
        {
            throw new NotImplementedException();
        }

        public override UserControl GetSettingsControl(IProject project)
        {
            throw new NotImplementedException();
        }
    }
}
