using AvalonStudio.CommandLineTools;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.GCC;
using AvalonStudio.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ClangCheck
    {
        private static string ContentDirectory => Path.Combine(PackageManager.GetPackageDirectory("AvalonStudio.Toolchains.Clang"), "content");
        private static string BinDirectory => Path.Combine(ContentDirectory, "bin");
        private static string ClangCheckCommand => Path.Combine(BinDirectory, "clang-tidy") + Platform.ExecutableExtension;

        public static async Task RunAsync(IConsole console, IStandardProject mainProject)
        {
            await PackageManager.EnsurePackage("AvalonStudio.Toolchains.Clang", console);

            mainProject.VisitSourceFiles((masterProject, project, file) =>
            {
                if(SupportsFile(file) && !file.Location.Contains("STM32F4xx_HAL_Driver") && !file.Location.Contains("STM32F4Cube"))
                {
                    console.WriteLine($"Running analysis on: {file.Location}");

                    var args = GetCompilerArguments(masterProject, project, file);
                    PlatformSupport.ExecuteShellCommand(ClangCheckCommand, $"{file.Location} -checks=* -- {args}", (s, e) => console.WriteLine(e.Data), (s, e) =>
                    {
                        if (e.Data != null)
                        {
                            console.WriteLine();
                            console.WriteLine(e.Data);
                        }
                    },
                    false, file.CurrentDirectory, false);
                }
            });
        }

        private static IEnumerable<string> GetToolchainIncludes(ISourceFile file)
        {
            return new List<string>
            {
                Path.Combine(ContentDirectory, "arm-none-eabi", "include", "c++", "6.3.1"),
                Path.Combine(ContentDirectory, "arm-none-eabi", "include", "c++", "6.3.1", "arm-none-eabi"),
                Path.Combine(ContentDirectory, "arm-none-eabi", "include", "c++", "6.3.1", "backward"),
                Path.Combine(ContentDirectory, "arm-none-eabi", "include"),
                Path.Combine(ContentDirectory, "lib", "gcc", "arm-none-eabi", "6.3.1", "include"),
                Path.Combine(ContentDirectory, "lib", "gcc", "arm-none-eabi", "6.3.1", "include-fixed"),
                Path.Combine(ContentDirectory, "arm-none-eabi", "include")
            };
        }

        private static string GetCompilerArguments(IStandardProject superProject, IStandardProject project, ISourceFile file)
        {
            var result = string.Empty;

            var settings = superProject.GetToolchainSettings<GccToolchainSettings>();

            if (file.Extension == ".cpp")
            {
                result = "-x c++ -fno-use-cxa-atexit ";
            }

            result += "-Wall -c -fshort-enums ";

            if (settings.CompileSettings.DebugInformation)
            {
                result += "-ggdb3 ";
            }

            if (file == null || file.Extension == ".cpp")
            {
                switch (settings.CompileSettings.CppLanguageStandard)
                {
                    case CppLanguageStandard.Cpp98:
                        result += "-std=c++98 ";
                        break;

                    case CppLanguageStandard.Cpp03:
                        result += "-std=c++03 ";
                        break;

                    case CppLanguageStandard.Cpp11:
                        result += "-std=c++11 ";
                        break;

                    case CppLanguageStandard.Cpp14:
                        result += "-std=c++14 ";
                        break;

                    case CppLanguageStandard.Cpp17:
                        result += "-std=c++17 ";
                        break;

                    default:
                        break;
                }
            }

            if (file == null || file.Extension == ".c")
            {
                switch (settings.CompileSettings.CLanguageStandard)
                {
                    case CLanguageStandard.C89:
                        result += "-std=c89 ";
                        break;

                    case CLanguageStandard.C99:
                        result += "-std=c99 ";
                        break;

                    case CLanguageStandard.C11:
                        result += "-std=c11 ";
                        break;
                }
            }

            switch (settings.CompileSettings.Fpu)
            {
                case FPUSupport.Soft:
                    result += "-mfpu=fpv4-sp-d16 -mfloat-abi=softfp ";
                    break;

                case FPUSupport.Hard:
                    result += "-mfpu=fpv4-sp-d16 -mfloat-abi=hard ";
                    break;
            }

            // TODO remove dependency on file?
            if (file != null)
            {
                if (file.Extension == ".cpp")
                {
                    if (!settings.CompileSettings.Rtti)
                    {
                        result += "-fno-rtti ";
                    }

                    if (!settings.CompileSettings.Exceptions)
                    {
                        result += "-fno-exceptions ";
                    }
                }
            }

            switch (settings.CompileSettings.Fpu)
            {
                case FPUSupport.Soft:
                    {
                        result += "-mfpu=fpv4-sp-d16 -mfloat-abi=softfp ";
                    }
                    break;

                case FPUSupport.Hard:
                    {
                        result += "-mfpu=fpv4-sp-d16 -mfloat-abi=hard ";
                    }
                    break;
            }

            // TODO make this an option.
            result += "-ffunction-sections -fdata-sections ";

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
                        result += "-Ofast ";
                    }
                    break;
            }

            result += settings.CompileSettings.CustomFlags + " ";

            // Referenced includes
            var referencedIncludes = project.GetReferencedIncludes();

            referencedIncludes.Select(s => result += $"-I\"{ Path.Combine(project.CurrentDirectory, s)}\" ").ToList();

            // global includes
            var globalIncludes = superProject.GetGlobalIncludes();

            globalIncludes.Select(s => result += $"-I\"{s}\" ").ToList();

            // includes
            project.Includes.Select(s => result += $"-I\"{ Path.Combine(project.CurrentDirectory, s.Value)}\" ").ToList();

            var referencedDefines = project.GetReferencedDefines();
            referencedDefines.Select(s => result += $"-D{s} ").ToList();

            var toolchainIncludes = GetToolchainIncludes(file);
            toolchainIncludes.Select(s => result += $"-isystem\"{s}\" ").ToList();

            // global includes
            var globalDefines = superProject.GetGlobalDefines();

            globalDefines.Select(s => result += $"-D{s} ").ToList();

            project.Defines.Select(s => result += $"-D{s.Value} ").ToList();

            superProject.ToolChainArguments.Select(s => result += $" {s}").ToList();

            superProject.CompilerArguments.Select(s => result += $" {s}").ToList();

            // TODO factor out this code from here!
            if (file != null)
            {
                switch (file.Extension)
                {
                    case ".c":
                        {
                            superProject.CCompilerArguments.Select(s => result += $" {s}");
                        }
                        break;

                    case ".cpp":
                        {
                            superProject.CppCompilerArguments.Select(s => result += $" {s}");
                        }
                        break;
                }
            }

            return result;
        }

        private static bool SupportsFile(ISourceFile file)
        {
            var result = false;

            switch (file.Extension.ToLower())
            {
                case ".cpp":
                case ".c":
                    result = true;
                    break;
            }

            return result;
        }
    }
}
