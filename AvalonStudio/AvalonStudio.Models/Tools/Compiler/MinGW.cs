using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using AvalonStudio.Models.Solutions;
namespace AvalonStudio.Models.Tools.Compiler
{
    public class MinGWToolChain : StandardToolChain
    {
        public override void Compile(IConsole console, Project superProject, Project project, ProjectFile file, string outputFile, CompileResult result)
        {
            var startInfo = new ProcessStartInfo();

            string binDirectory = Path.Combine(Settings.ToolChainLocation, "bin");

            startInfo.FileName = Path.Combine(binDirectory, "gcc.exe");
            startInfo.EnvironmentVariables["Path"] = AppendPath(startInfo.EnvironmentVariables["Path"], binDirectory);

            startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

            if (!File.Exists(startInfo.FileName))
            {
                result.ExitCode = -1;
                console.WriteLine("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
                return;
            }

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            string fileArguments = string.Empty;

            if (file.FileType == FileType.CPlusPlus)
            {
                fileArguments = "-x c++ -std=c++14 -fno-use-cxa-atexit";
            }

            startInfo.Arguments = string.Format("{0} {1} {2} -o{3} -MMD -MP", GetCompilerArguments(superProject, file.FileType), fileArguments, file.Location, outputFile);

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            //console.WriteLine("[CC] - " + Path.GetFileName(file.Location) + startInfo.Arguments);

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
        }

        public override LinkResult Link (IConsole console, Project superProject, Project project, CompileResult assemblies, string outputDirectory)
        {
            LinkResult result = new LinkResult();

            ProcessStartInfo startInfo = new ProcessStartInfo();

            string binDirectory = Path.Combine(Settings.ToolChainLocation, "bin");

            startInfo.FileName = Path.Combine(binDirectory, "g++.exe");
            startInfo.EnvironmentVariables["Path"] = AppendPath(startInfo.EnvironmentVariables["Path"], binDirectory);

            if (project.SelectedConfiguration.IsLibrary)
            {
                startInfo.FileName = Path.Combine(binDirectory, "ar.exe");
            }

            startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

            if (!File.Exists(startInfo.FileName))
            {
                result.ExitCode = -1;
                console.WriteLine("Unable to find linker executable (" + startInfo.FileName + ") Check project compiler settings.");
                return result;
            }

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

            string outputName = Path.GetFileNameWithoutExtension(project.FileName);

            if (outputName.Length > 10)
            {
                outputName = outputName.Substring(0, 10); //Trim the filename otherwise it will require admin priviledges to run.
            }

            outputName += ".exe";

            if (project.SelectedConfiguration.IsLibrary)
            {
                outputName = "lib" + Path.GetFileNameWithoutExtension(project.FileName) + ".a";
            }

            var executable = Path.Combine(outputDirectory, outputName);

            string linkedLibraries = string.Empty;

            foreach (var libraryPath in project.SelectedConfiguration.LinkedLibraries)
            {
                string relativePath = Path.GetDirectoryName(libraryPath);

                string libName = Path.GetFileNameWithoutExtension(libraryPath).Substring(3);

                linkedLibraries += string.Format(" -L\"{0}\" -l{1}", relativePath, libName);
            }

            linkedLibraries = " " + linkedLibraries.Trim();

            // TODO linked libraries won't make it in on nano... Please fix -L directory placement in compile string.
            //switch (project.SelectedConfiguration.Library)
            //{
            //    case LibraryType.NanoCLib:
            //        linkedLibraries = " -lm -lc_nano -lsupc++_nano -lstdc++_nano";
            //        break;

            //    case LibraryType.BaseCLib:
            //        linkedLibraries += " -lm -lgcc";
            //        break;

            //    case LibraryType.SemiHosting:
            //        linkedLibraries += " -lm -lgcc -lc -lrdimon";
            //        break;

            //    case LibraryType.Retarget:
            //        linkedLibraries += " -lm -lgcc -lnosys -lstdc++";
            //        break;
            //}

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;

            startInfo.Arguments = string.Format("{0} -o{1} {2} -Wl,--start-group {3} {4} -Wl,--end-group", GetLinkerArguments(project), executable, objectArguments, libs, linkedLibraries);

            if (project.SelectedConfiguration.IsLibrary)
            {
                startInfo.Arguments = string.Format("rvs {0} {1}", executable, objectArguments);
            }

            //console.WriteLine("[LL] - " + startInfo.Arguments);

            using (var process = Process.Start(startInfo))
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    //console.WriteLine(e.Data);
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    console.WriteLine(e.Data);
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

        public override ProcessResult Size (IConsole console, Project project, LinkResult linkResult)
        {
            ProcessResult result = new ProcessResult ();

            ProcessStartInfo startInfo = new ProcessStartInfo ();

            string binDirectory = Path.Combine (Settings.ToolChainLocation, "bin");
            startInfo.FileName = Path.Combine (binDirectory, "size.exe");
            startInfo.EnvironmentVariables ["Path"] = AppendPath (startInfo.EnvironmentVariables ["Path"], binDirectory);

            if (!File.Exists (startInfo.FileName))
            {
                console.WriteLine ("Unable to find tool (" + startInfo.FileName + ") check project compiler settings.");
                result.ExitCode = -1;
                return result;
            }

            startInfo.Arguments = string.Format ("{0}", linkResult.Executable);

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;


            using (var process = Process.Start (startInfo))
            {
                if (console != null)
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var task = process.StandardOutput.ReadToEndAsync ();
                        task.Wait ();
                        console.Write (task.Result);
                    }
                }

                process.WaitForExit ();

                result.ExitCode = process.ExitCode;
            }

            return result;
        }

        public override string GetCompilerArguments(Project project, FileType language)
        {
            string result = string.Empty;
            var configuration = project.SelectedConfiguration;

            string targetOptions = string.Empty;

            string fpu = string.Empty;

            string standardOptions = " -Wall -c -Wno-unknown-pragmas";

            //if(!project.SelectedConfiguration.IsLibrary)
            {
                standardOptions += " -ffunction-sections -fdata-sections";
            }

            string optimizationLevel = string.Empty;

            switch (configuration.Optimization)
            {
                case OptimizationLevel.None:
                    optimizationLevel = " -O0";
                    break;

                case OptimizationLevel.Debug:
                    optimizationLevel = " -Og";
                    break;

                case OptimizationLevel.Level1:
                    optimizationLevel = " -O1";
                    break;

                case OptimizationLevel.Level2:
                    optimizationLevel = " -O2";
                    break;

                case OptimizationLevel.Level3:
                    optimizationLevel = " -O3";
                    break;
            }

            string optimizationPreference = string.Empty;

            switch (configuration.OptimizationPreference)
            {
                case OptimizationPreference.Size:
                    optimizationPreference = " -Os";
                    break;

                case OptimizationPreference.Speed:
                    optimizationPreference = " -Ofast";
                    break;
            }

            string miscOptions = " " + configuration.MiscCompilerArguments;

            string defines = string.Empty;

            foreach (var define in configuration.Defines)
            {
                defines += string.Format(" -D{0}", define);
            }

            string includes = " ";

            foreach (var include in project.IncludeArguments)
            {
                includes += string.Format(" {0} ", include);
            }

            string outputOptions = string.Empty;

            if (configuration.DebugSymbols)
            {
                outputOptions = " -g";
            }

            if (language == FileType.CPlusPlus)
            {
                if (!configuration.Rtti)
                {
                    outputOptions += " -fno-rtti";
                }

                if (!configuration.Exceptions)
                {
                    outputOptions += " -fno-exceptions";
                }
            }

            result = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", outputOptions, targetOptions, fpu, standardOptions, optimizationLevel, optimizationPreference, miscOptions, defines, includes);

            return result;
        }

        public override string GetLinkerArguments(Project project)
        {
            string result = string.Empty;                        

            result += string.Format("-flto -static-libgcc -static-libstdc++ -Wl,-Map={0}.map ", Path.GetFileNameWithoutExtension(project.FileName));            

            if (project.SelectedConfiguration.DiscardUnusedSections)
            {
                result += "-Wl,--gc-sections ";
            }

            string optimizationLevel = string.Empty;

            switch (project.SelectedConfiguration.Optimization)
            {
                case OptimizationLevel.None:
                    result += " -O0";
                    break;

                case OptimizationLevel.Level1:
                    result += " -O1";
                    break;

                case OptimizationLevel.Level2:
                    result += " -O2";
                    break;

                case OptimizationLevel.Level3:
                    result += " -O3";
                    break;
            }

            result += " " + project.SelectedConfiguration.MiscLinkerArguments;            

            return result;
        }

        public enum MinGWOptimizationLevel
        {
            [Description ("-O0")]
            Off,
            [Description ("-O1")]
            Level1,
            [Description ("-O2")]
            Level2,
            [Description ("-O3")]
            Level3
        }

        public enum MinGWOptimizationPreference
        {
            //[Description ("")]
            None,
            //[Description ("-Os")]
            Size,
            //[Description ("-Ofast")]
            Speed
        }

        public MinGWOptimizationLevel Optimization { get; set; }
        public MinGWOptimizationPreference OptimizationPriority { get; set; }

        public string CompilerCustomArguments { get; set; }

        public string LinkerCustomArguments { get; set; }

        public string LinkerScript { get; set; }

        public override string GDBExecutable
        {
            get
            {
                string binDirectory = Path.Combine (Settings.ToolChainLocation, "bin");
                return Path.Combine (binDirectory, "gdb.exe");
            }
        }
    }
}
