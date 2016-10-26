using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.Utils;
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

        public override CompileResult Compile(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string outputFile)
        {
            var result = new CompileResult();

            var startInfo = new ProcessStartInfo();

            if (file.Extension == ".cpp")
            {
                startInfo.FileName = Path.Combine(BinDirectory, $"{CCPPrefix}{CCPPName}" + Platform.ExecutableExtension);
            }
            else
            {
                startInfo.FileName = Path.Combine(BinDirectory, $"{CCPPrefix}{CCName}" + Platform.ExecutableExtension);
            }

            startInfo.EnvironmentVariables["Path"] = BinDirectory;
            startInfo.WorkingDirectory = file.CurrentDirectory;

            if (Path.IsPathRooted(startInfo.FileName) && !System.IO.File.Exists(startInfo.FileName))
            {
                result.ExitCode = -1;
                console.WriteLine("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
            }
            else
            {
                var fileArguments = string.Empty;

                if (file.Extension == ".cpp")
                {
                    fileArguments = "-x c++ -fno-use-cxa-atexit";
                }

                startInfo.Arguments = string.Format("{0} {1} {2} -o{3} -MMD -MP", fileArguments,
                    GetCompilerArguments(superProject, project, file), file.Location, outputFile);

                // Hide console window
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;

                //console.WriteLine (Path.GetFileNameWithoutExtension(startInfo.FileName) + " " + startInfo.Arguments);

                using (var process = Process.Start(startInfo))
                {
                    process.OutputDataReceived += (sender, e) => { console.WriteLine(e.Data); };

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

        public override LinkResult Link(IConsole console, IStandardProject superProject, IStandardProject project, CompileResult assemblies, string outputPath)
        {
            var result = new LinkResult();

            var startInfo = new ProcessStartInfo();

            startInfo.FileName = Path.Combine(BinDirectory, $"{LDPrefix}{LDName}" + Platform.ExecutableExtension);

            if (project.Type == ProjectType.StaticLibrary)
            {
                startInfo.FileName = Path.Combine(BinDirectory, $"{ARPrefix}{ARName}" + Platform.ExecutableExtension);
            }

            startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

            if (Path.IsPathRooted(startInfo.FileName) && !System.IO.File.Exists(startInfo.FileName))
            {
                result.ExitCode = -1;
                console.WriteLine("Unable to find linker executable (" + startInfo.FileName + ") Check project compiler settings.");
                return result;
            }

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

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;

            if (project.Type == ProjectType.StaticLibrary)
            {
                startInfo.Arguments = string.Format("rvs {0} {1}", executable, objectArguments);
            }
            else
            {
                startInfo.Arguments = string.Format("{0} -o{1} {2} -Wl,--start-group {3} {4} -Wl,--end-group", GetLinkerArguments(superProject, project), executable, objectArguments, linkedLibraries, libs);
            }

            //console.WriteLine(Path.GetFileNameWithoutExtension(startInfo.FileName) + " " + startInfo.Arguments);            

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
            var result = new ProcessResult();

            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.Combine(BinDirectory, $"{SizePrefix}{SizeName}" + Platform.ExecutableExtension);

            if (Path.IsPathRooted(startInfo.FileName) && !System.IO.File.Exists(startInfo.FileName))
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
                process.OutputDataReceived += (sender, e) => { console.WriteLine(e.Data); };

                process.ErrorDataReceived += (sender, e) => { console.WriteLine(e.Data); };

                process.BeginOutputReadLine();

                process.BeginErrorReadLine();

                process.WaitForExit();

                result.ExitCode = process.ExitCode;
            }

            return result;
        }
    }
}