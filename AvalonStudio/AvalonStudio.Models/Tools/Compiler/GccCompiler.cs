using System.Diagnostics;
using System.IO;
namespace AvalonStudio.Models.Tools.Compiler
{
    using AvalonStudio.Models.Solutions;
    using System.Collections.Generic;
    using Templates;

    public class GCCToolChain : StandardToolChain
    {
        public GCCToolChain ()
        {
            Settings.IncludePaths = new List<string>()
            {
                "c:\\VEStudio\\AppData\\Repos\\GCCToolChain\\arm-none-eabi\\include",
                "c:\\VEStudio\\AppData\\Repos\\GCCToolChain\\arm-none-eabi\\include\\c++\\4.9.3",
                "c:\\VEStudio\\AppData\\Repos\\GCCToolChain\\arm-none-eabi\\c++\\4.9.3\\arm-none-eabi\\thumb",
                "c:\\VEStudio\\AppData\\Repos\\GCCToolChain\\lib\\gcc\\arm-none-eabi\\4.9.3\\include"
            };
        }

        public override void Compile (IConsole console, Project superProject, Project project, ProjectFile file, string outputFile, CompileResult result)
        {
            var startInfo = new ProcessStartInfo ();

            string binDirectory = Path.Combine (Settings.ToolChainLocation, "bin");

            startInfo.FileName = Path.Combine (binDirectory, "arm-none-eabi-gcc.exe");

            startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

            if (!File.Exists (startInfo.FileName))
            {
                result.ExitCode = -1;
                console.WriteLine ("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
                return;
            }

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;            

            string fileArguments = string.Empty;

            if(file.FileType == FileType.CPlusPlus)
            {
                fileArguments = "-x c++ -std=c++14 -fno-use-cxa-atexit";
            }

            startInfo.Arguments = string.Format ("{0} {1} {2} -o{3} -MMD -MP", GetCompilerArguments (superProject, file.FileType), fileArguments, file.Location, outputFile);

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            //console.WriteLine ("[CC] - " + Path.GetFileName (file.Location) + startInfo.Arguments);

            using (var process = Process.Start (startInfo))
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

                process.WaitForExit ();
                
                result.ExitCode = process.ExitCode;
            }
        }

        public override LinkResult Link (IConsole console, Project superProject, Project project, CompileResult assemblies, string outputDirectory)
        {
            LinkResult result = new LinkResult ();

            ProcessStartInfo startInfo = new ProcessStartInfo ();

            string binDirectory = Path.Combine (Settings.ToolChainLocation, "bin");

            startInfo.FileName = Path.Combine (binDirectory, "arm-none-eabi-gcc.exe");

            if (project.SelectedConfiguration.IsLibrary)
            {
                startInfo.FileName = Path.Combine (binDirectory, "arm-none-eabi-ar.exe");
            }

            startInfo.WorkingDirectory = project.Solution.CurrentDirectory;

            if (!File.Exists (startInfo.FileName))
            {
                result.ExitCode = -1;
                console.WriteLine ("Unable to find linker executable (" + startInfo.FileName + ") Check project compiler settings.");
                return result;
            }

            GenerateLinkerScript (project);

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

            if (!Directory.Exists (outputDirectory))
            {
                Directory.CreateDirectory (outputDirectory);
            }

            string outputName = Path.GetFileNameWithoutExtension (project.FileName) + ".elf";

            if (project.SelectedConfiguration.IsLibrary)
            {
                outputName = "lib" + Path.GetFileNameWithoutExtension (project.FileName) + ".a";
            }

            var executable = Path.Combine (outputDirectory, outputName);

            string linkedLibraries = string.Empty;

            foreach (var libraryPath in project.SelectedConfiguration.LinkedLibraries)
            {
                string relativePath = Path.GetDirectoryName (libraryPath);

                string libName = Path.GetFileNameWithoutExtension (libraryPath).Substring (3);

                linkedLibraries += string.Format (" -L\"{0}\" -l{1}", relativePath, libName);
            }

            linkedLibraries = " " + linkedLibraries.Trim ();            

            // TODO linked libraries won't make it in on nano... Please fix -L directory placement in compile string.
            switch (project.SelectedConfiguration.Library)
            {
                case LibraryType.NanoCLib:
					linkedLibraries = " -lm -lc_nano -lsupc++_nano -lstdc++_nano";
                    break;

                case LibraryType.BaseCLib:
                    linkedLibraries += " -lm -lc -lstdc++ -lsupc++";
                    break;

                case LibraryType.SemiHosting:
					linkedLibraries += " -lm -lgcc -lc -lrdimon";
                    break;

                case LibraryType.Retarget:
					linkedLibraries += " -lm -lc -lnosys -lstdc++ -lsupc++";
                    break;
            }

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

            //console.WriteLine ("[LL] - " + startInfo.Arguments);

            using (var process = Process.Start (startInfo))
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
            startInfo.FileName = Path.Combine (binDirectory, "arm-none-eabi-size.exe");

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
        
        public override string GetLinkerArguments (Project project)
        {
            string result = string.Empty;

            result += string.Format ("-mcpu={0} -mthumb ", project.SelectedConfiguration.Mcpu);

            switch (project.SelectedConfiguration.Fpu)
            {
                case FPUSupport.Soft:
                    result += " -mfpu=fpv4-sp-d16 -mfloat-abi=softfp ";
                    break;

                case FPUSupport.Hard:
                    result += " -mfpu=fpv4-sp-d16 -mfloat-abi=hard ";
                    break;
            }

            result += string.Format("-flto -Wl,-Map={0}.map ", Path.GetFileNameWithoutExtension(project.FileName));

            if(project.SelectedConfiguration.NotUseStandardStartupFiles)
            {
                result += "-nostartfiles ";
            }

            if(project.SelectedConfiguration.DiscardUnusedSections)
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

            result += string.Format (" -L{0} -Wl,-T\"{1}\"", Path.GetDirectoryName (GetLinkerScriptLocation (project)), Path.GetFileName (GetLinkerScriptLocation (project)));            
                       
            return result;
        }

        public override string GetCompilerArguments (Project project, FileType language)
        {
            string result = string.Empty;
            var configuration = project.SelectedConfiguration;

            string targetOptions = string.Empty;

            targetOptions += " -mthumb";
            
            //TODO add each option to a table in db and reference.
             //‘arm2’, ‘arm250’, ‘arm3’, ‘arm6’, ‘arm60’, ‘arm600’, ‘arm610’, ‘arm620’, ‘arm7’, ‘arm7m’, ‘arm7d’, ‘arm7dm’, ‘arm7di’, ‘arm7dmi’, ‘arm70’, ‘arm700’, ‘arm700i’, ‘arm710’, ‘arm710c’, ‘arm7100’, ‘arm720’, ‘arm7500’, ‘arm7500fe’, ‘arm7tdmi’, ‘arm7tdmi-s’, ‘arm710t’, ‘arm720t’, ‘arm740t’, ‘strongarm’, ‘strongarm110’, ‘strongarm1100’, ‘strongarm1110’, ‘arm8’, ‘arm810’, ‘arm9’, ‘arm9e’, ‘arm920’, ‘arm920t’, ‘arm922t’, ‘arm946e-s’, ‘arm966e-s’, ‘arm968e-s’, ‘arm926ej-s’, ‘arm940t’, ‘arm9tdmi’, ‘arm10tdmi’, ‘arm1020t’, ‘arm1026ej-s’, ‘arm10e’, ‘arm1020e’, ‘arm1022e’, ‘arm1136j-s’, ‘arm1136jf-s’, ‘mpcore’, ‘mpcorenovfp’, ‘arm1156t2-s’, ‘arm1156t2f-s’, ‘arm1176jz-s’, ‘arm1176jzf-s’, ‘cortex-a5’, ‘cortex-a7’, ‘cortex-a8’, ‘cortex-a9’, ‘cortex-a12’, ‘cortex-a15’, ‘cortex-a53’, ‘cortex-a57’, ‘cortex-a72’, ‘cortex-r4’, ‘cortex-r4f’, ‘cortex-r5’, ‘cortex-r7’, ‘cortex-m7’, ‘cortex-m4’, ‘cortex-m3’, ‘cortex-m1’, ‘cortex-m0’, ‘cortex-m0plus’, ‘cortex-m1.small-multiply’, ‘cortex-m0.small-multiply’, ‘cortex-m0plus.small-multiply’, ‘marvell-pj4’, ‘xscale’, ‘iwmmxt’, ‘iwmmxt2’, ‘ep9312’, ‘fa526’, ‘fa626’, ‘fa606te’, ‘fa626te’, ‘fmp626’, ‘fa726te’, ‘xgene1’.
            targetOptions += string.Format (" -mcpu={0}", configuration.Mcpu);           
            
            string fpu = string.Empty;

            switch (configuration.Fpu)
            {
                case FPUSupport.Soft:
                    fpu = " -mfpu=fpv4-sp-d16 -mfloat-abi=soft ";
                    break;

                case FPUSupport.Hard:
                    fpu = " -mfpu=fpv4-sp-d16 -mfloat-abi=hard ";
                    break;
            }

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
                defines += string.Format (" -D{0}", define);
            }

            string includes = " ";

            foreach (var include in project.IncludeArguments)
            {
                includes += string.Format (" {0} ", include);
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

            result = string.Format ("{0}{1}{2}{3}{4}{5}{6}{7}{8}", outputOptions, targetOptions, fpu, standardOptions, optimizationLevel, optimizationPreference, miscOptions, defines, includes);

            return result;
        }

        public void GenerateLinkerScript (Project project)
        {
            var template = new ArmGCCLinkTemplate (project.SelectedConfiguration);

            string linkerScript = GetLinkerScriptLocation (project);

            if (File.Exists (linkerScript))
            {
                File.Delete (linkerScript);
            }

            var sw = File.CreateText (linkerScript);

            sw.Write (template.TransformText ());

            sw.Close ();
        }

        private string GetLinkerScriptLocation (Project project)
        {
            return Path.Combine (project.CurrentDirectory, "link.ld");
        }

        public override string GDBExecutable
        {
            get
            {
                string binDirectory = Path.Combine (Settings.ToolChainLocation, "bin");
                return Path.Combine (binDirectory, "arm-none-eabi-gdb.exe");
            }
        }
    }
}
