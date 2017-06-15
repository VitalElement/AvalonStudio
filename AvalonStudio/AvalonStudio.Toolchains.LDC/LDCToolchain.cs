using AvalonStudio.Toolchains.GCC;
using System;
using System.Collections.Generic;
using System.Text;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Utils;
using System.Threading.Tasks;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.CommandLineTools;
using System.IO;
using AvalonStudio.Packages;

namespace AvalonStudio.Toolchains.LDC
{
    public class LDCToolchain : GCCToolchain
    {
        public static string ContentDirectory => Path.Combine(PackageManager.GetPackageDirectory("AvalonStudio.Toolchains.Clang"), "content");
        public override string BinDirectory => Path.Combine(ContentDirectory, "bin");

        public override string CCName => "ldc2";

        public override string ExecutableExtension => ".elf";

        public override string StaticLibraryExtension => ".a";

        public override Version Version => new Version();

        public override string Description => "LLVM Based D Toolchain";        

        public override bool ValidateToolchainExecutables(IConsole console)
        {
            bool result = true;

            result = CheckFile(console, CCExecutable);

            return result;
        }

        public override bool CanHandle(IProject project)
        {
            return project is CPlusPlusProject;
        }               

        public override string GetBaseLibraryArguments(IStandardProject superProject)
        {
            return string.Empty;
        }

        public override string GetCompilerArguments(IStandardProject superProject, IStandardProject project, ISourceFile sourceFile)
        {
            var result = string.Empty;

            var settings = superProject.GetToolchainSettings<GccToolchainSettings>();

            result += "-c ";

            switch (settings.CompileSettings.Optimization)
            {
                case OptimizationLevel.None:
                    {
                        result += "-O0 ";
                    }
                    break;

                case OptimizationLevel.Debug:
                    {
                        result += "-O2 ";
                    }
                    break;

                case OptimizationLevel.Level1:
                    {
                        result += "-O1 ";
                    }
                    break;

                case OptimizationLevel.Level2:
                    {
                        result += "-O2 ";
                    }
                    break;

                case OptimizationLevel.Level3:
                    {
                        result += "-O3 ";
                    }
                    break;
            }

            switch (settings.CompileSettings.OptimizationPreference)
            {
                case OptimizationPreference.Size:
                    {
                        result += "-Os ";
                    }
                    break;

                case OptimizationPreference.Speed:
                    {
                        result += "-O3 ";
                    }
                    break;
            }

            result += settings.CompileSettings.CustomFlags + " ";

            return result;
        }

        public override IList<object> GetConfigurationPages(IProject project)
        {
            var result = new List<object>();

            result.Add(new CompileSettingsFormViewModel(project));
            result.Add(new LinkerSettingsFormViewModel(project));

            return result;

        }

        public override string GetLinkerArguments(IStandardProject superProject, IStandardProject project)
        {
            return string.Empty;
        }

        public override IEnumerable<string> GetToolchainIncludes(ISourceFile file)
        {
            return new List<string>();
        }

        public override Task InstallAsync(IConsole console, IProject project)
        {
            await PackageManager.EnsurePackage("AvalonStudio.Toolchains.Clang", (project as CPlusPlusProject).ToolchainVersion, console);
        }

        public override CompileResult Compile(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string outputFile)
        {
            var result = new CompileResult();

            string commandName = CCExecutable;

            var fileArguments = string.Empty;

            var arguments = string.Format("{0} {1} {2} -of{3}", fileArguments, GetCompilerArguments(superProject, project, file), file.Location, outputFile);

            result.ExitCode = PlatformSupport.ExecuteShellCommand(commandName, arguments, (s, e) => console.WriteLine(e.Data), (s, e) =>
            {
                if (e.Data != null)
                {
                    console.WriteLine();
                    console.WriteLine(e.Data);
                }
            },
            false, file.CurrentDirectory, false);

            // console.WriteLine(Path.GetFileNameWithoutExtension(commandName) + " " + arguments);

            return result;
        }
    }
}
