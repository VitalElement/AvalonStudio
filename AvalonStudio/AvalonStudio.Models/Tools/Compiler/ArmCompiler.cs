namespace AvalonStudio.Models.Tools.Compiler
{
    using Solutions;
    using Utils;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    public class Arm : StandardToolChain
    {        
        public override void Compile (IConsole console, Project superProject, Project project, ProjectFile file, string outputFile, CompileResult result)
        {
            var startInfo = new ProcessStartInfo ();

            startInfo.FileName = Path.Combine (project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "armcc.exe");

            if (!File.Exists (startInfo.FileName))
            {
                result.ExitCode = -1;
                console.WriteLine ("Unable to find compiler (" + startInfo.FileName + ") Please check project compiler settings.");
                return;
            }

            console.WriteLine ("Compiling: " + file.FileName);

            string includes = string.Empty;
            foreach (string include in file.Project.IncludeArguments)
            {
                includes += string.Format ("{0} ", include);
            }

            string defines = string.Empty;
            foreach (string define in file.Project.SelectedConfiguration.Defines)
            {
                defines += string.Format ("-D{0} ", define);
            }
           
            string targetArguments = string.Empty;
            //string mcpu = file.Project.SelectedConfiguration.Target.MCpu;

            //if (file.Project.SelectedConfiguration.Target is ArmTarget)
            //{
            //    var armTarget = file.Project.SelectedConfiguration.Target as ArmTarget;

            //    if (armTarget.Thumb)
            //    {
            //        targetArguments += "--thumb ";
            //    }

            //    if (armTarget.Fpu == ArmTarget.FloatingPointUnitType.Hard)
            //    {
            //        // do nothing.
            //    }
            //    else
            //    {
            //        mcpu += ".fp";
            //    }
            //}

            startInfo.Arguments = string.Format ("-c --cpu={0} {1} {2} {3} {4} {5} {6} -o{7} {8}",
                "",
                targetArguments,
                this.CompilerCustomArguments,
                includes,
                defines,
                this.Optimization.GetDescription(),
                this.OptimizationPriority.GetDescription(),
                outputFile,
                file.Location);

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            using (var process = Process.Start (startInfo))
            {
                if (console != null)
                {
                    Task.Factory.StartNew (() =>
                    {
                        while (!process.StandardOutput.EndOfStream)
                        {
                            console.WriteLine (process.StandardOutput.ReadLine ());
                        }
                    });


                    Task.Factory.StartNew (() =>
                    {
                        while (!process.StandardError.EndOfStream)
                        {
                            console.WriteLine (process.StandardError.ReadLine ());
                        }
                    });
                }

                process.WaitForExit ();

                result.ExitCode = process.ExitCode;
            }
        }

        public override LinkResult Link (IConsole console,Project superProject,  Project project, CompileResult assemblies, string outputDirectory)
        {
            LinkResult result = new LinkResult ();

            ProcessStartInfo startInfo = new ProcessStartInfo ();

            startInfo.FileName = Path.Combine (project.SelectedConfiguration.ToolChain.Settings.ToolChainLocation, "armlink.exe");

            if (!File.Exists (startInfo.FileName))
            {
                console.WriteLine ("Unable to find linker executable. (" + startInfo.FileName + ") please check VEStudio toolcahin settings.");
                result.ExitCode = -1;
                return result;
            }

            if (this.LinkerScript == null)
            {
                result.ExitCode = -1;
                console.WriteLine ("Unable to find linker script. Check project linker settings.");
                return result;
            }

            string objectArguments = string.Empty;
            foreach (string obj in assemblies.ObjectLocations)
            {
                objectArguments += obj + " ";
            }

            string outputName = Path.GetFileNameWithoutExtension (project.FileName) + ".elf";
            var executable = Path.Combine (outputDirectory, outputName);

            startInfo.Arguments = string.Format ("{0} -o{1} {2}",
                this.LinkerCustomArguments,
                executable,
                objectArguments);

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

                if (result.ExitCode == 0)
                {
                    result.Executable = executable;
                }
            }

            return result;
        }

        public override ProcessResult Size (IConsole console, Solutions.Project project, LinkResult linkResult)
        {
            console.WriteLine ("Not implemented arm size tool.");
            return new ProcessResult () { ExitCode = -1 };
        }

        public override string GetCompilerArguments(Project project, FileType language)
        {            
            return "Arm Toolchain currently out of service. Contact developers.";
        }

        public override string GetLinkerArguments(Project project)
        {
            return "Arm Toolchain currently out of service. Contact developers.";
        }

        #region Settings
        public enum ArmOptimizationLevel
        {
            [Description("-O0")]
            Off,
            [Description("-O1")]
            Level1,
            [Description("-O2")]
            Level2,
            [Description("-O3")]
            Level3
        }

        public enum ArmOptimizationPreference
        {
            [Description("")]
            None,
            [Description("-Osize")]
            Size,
            [Description("-Otime")]
            Speed
        }

        public ArmOptimizationLevel Optimization { get; set; }
        public ArmOptimizationPreference OptimizationPriority { get; set; }

        public string CompilerCustomArguments { get; set; }

        public string LinkerCustomArguments { get; set; }

        public string LinkerScript { get; set; }
        #endregion

        public override string GDBExecutable
        {
            get { return string.Empty; }
        }
    }
}
