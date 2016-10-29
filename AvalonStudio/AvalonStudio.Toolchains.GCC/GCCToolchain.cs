using AvalonStudio.CommandLineTools;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace AvalonStudio.Toolchains.GCC
{
    public abstract class GCCToolchain : StandardToolChain
    {
        public abstract string GDBExecutable { get; }

        public abstract string BinDirectory { get; }

        public abstract string GetBaseLibraryArguments(IStandardProject superProject);

        public virtual string Prefix => string.Empty;

        public virtual string CCPrefix => Prefix;

        public virtual string CCPPrefix => Prefix;

        public virtual string LDPrefix => Prefix;

        public virtual string ARPrefix => Prefix;

        public virtual string SizePrefix => Prefix;

        public virtual string CCName => "gcc";

        public virtual string CCPPName => "g++";

        public virtual string LDName => "gcc";

        public virtual string ARName => "ar";

        public virtual string SizeName => "size";

        public string CCExecutable => Path.Combine(BinDirectory, $"{CCPPrefix}{CCName}" + Platform.ExecutableExtension);

        public string CPPExecutable => Path.Combine(BinDirectory, $"{CCPPrefix}{CCPPName}" + Platform.ExecutableExtension);

        public string ARExecutable => Path.Combine(BinDirectory, $"{ARPrefix}{ARName}" + Platform.ExecutableExtension);

        public string LDExecutable => Path.Combine(BinDirectory, $"{LDPrefix}{LDName}" + Platform.ExecutableExtension);

        public string SizeExecutable => Path.Combine(BinDirectory, $"{SizePrefix}{SizeName}" + Platform.ExecutableExtension);

        public override CompileResult Compile(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string outputFile)
        {
            var result = new CompileResult();

            string commandName = file.Extension == ".cpp" ? CPPExecutable : CCExecutable;

            if (PlatformSupport.CheckExecutableAvailability(commandName, BinDirectory))
            {
                var fileArguments = string.Empty;

                if (file.Extension == ".cpp")
                {
                    fileArguments = "-x c++ -fno-use-cxa-atexit";
                }

                var arguments = string.Format("{0} {1} {2} -o{3} -MMD -MP", fileArguments, GetCompilerArguments(superProject, project, file), file.Location, outputFile);

                result.ExitCode = PlatformSupport.ExecuteShellCommand(commandName, arguments, (s, e) => console.WriteLine(e.Data), (s, e) =>
                {
                    if (e.Data != null)
                    {
                        console.WriteLine();
                        console.WriteLine(e.Data);
                    }
                },
                false, file.CurrentDirectory, false, BinDirectory);

                //console.WriteLine(Path.GetFileNameWithoutExtension(commandName) + " " + arguments);
            }
            else
            {
                console.WriteLine("Unable to find compiler (" + commandName + ") Please check project compiler settings.");

                result.ExitCode = -1;
            }

            return result;
        }

        public override LinkResult Link(IConsole console, IStandardProject superProject, IStandardProject project, CompileResult assemblies, string outputPath)
        {
            var result = new LinkResult();

            string commandName = project.Type == ProjectType.StaticLibrary ? ARExecutable : LDExecutable;

            if (PlatformSupport.CheckExecutableAvailability(commandName, BinDirectory))
            {
                var objectArguments = string.Empty;
                foreach (var obj in assemblies.ObjectLocations)
                {
                    objectArguments += obj + " ";
                }

                var libs = string.Empty;
                foreach (var lib in assemblies.LibraryLocations)
                {
                    libs += lib + " ";
                }

                var outputDir = Path.GetDirectoryName(outputPath);

                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                var outputName = Path.GetFileNameWithoutExtension(outputPath) + ExecutableExtension;

                if (project.Type == ProjectType.StaticLibrary)
                {
                    outputName = Path.GetFileNameWithoutExtension(outputPath) + StaticLibraryExtension;
                }

                var executable = Path.Combine(outputDir, outputName);

                var linkedLibraries = string.Empty;

                foreach (var libraryPath in project.StaticLibraries)
                {
                    var relativePath = Path.GetDirectoryName(libraryPath);

                    var libName = Path.GetFileNameWithoutExtension(libraryPath).Substring(3);

                    linkedLibraries += string.Format("-L\"{0}\" -l{1} ", relativePath, libName);
                }

                foreach (var lib in project.BuiltinLibraries)
                {
                    linkedLibraries += string.Format("-l{0} ", lib);
                }

                linkedLibraries += GetBaseLibraryArguments(superProject);

                string arguments = string.Empty;

                if (project.Type == ProjectType.StaticLibrary)
                {
                    arguments = string.Format("rvs {0} {1}", executable, objectArguments);
                }
                else
                {
                    arguments = string.Format("{0} -o{1} {2} -Wl,--start-group {3} {4} -Wl,--end-group", GetLinkerArguments(superProject, project), executable, objectArguments, linkedLibraries, libs);
                }

                result.ExitCode = PlatformSupport.ExecuteShellCommand(commandName, arguments, (s, e) => { },
                    (s, e) =>
                {
                    if (e.Data != null && !e.Data.Contains("creating"))
                    {
                        console.WriteLine(e.Data);
                    }
                }, false, project.Solution.CurrentDirectory, false, BinDirectory);

                //console.WriteLine(Path.GetFileNameWithoutExtension(commandName) + " " + arguments);

                if (result.ExitCode == 0)
                {
                    result.Executable = executable;
                }
            }
            else
            {
                result.ExitCode = -1;
                console.WriteLine("Unable to find linker executable (" + commandName + ") Check project compiler settings.");
            }

            return result;
        }

        public override ProcessResult Size(IConsole console, IStandardProject project, LinkResult linkResult)
        {
            var result = new ProcessResult();

            if (PlatformSupport.CheckExecutableAvailability(SizeExecutable, BinDirectory))
            {
                result.ExitCode = PlatformSupport.ExecuteShellCommand(SizeExecutable, linkResult.Executable,
                    (s, e) => console.WriteLine(e.Data),
                    (s, e) => console.WriteLine(e.Data),
                    false, string.Empty, false, BinDirectory);
            }
            else
            {
                console.WriteLine("Unable to find tool (" + SizeExecutable + ") check project compiler settings.");
                result.ExitCode = -1;
                return result;
            }

            return result;
        }
    }
}