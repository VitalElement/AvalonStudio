using Avalonia.Threading;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Controls.Standard.ErrorList;
using AvalonStudio.Extensibility;
using AvalonStudio.Languages;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Shell;
using AvalonStudio.Toolchains.GCC;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ClangTidyReplacement
    {
        public string FilePath { get; set; }
        public int  Offset { get; set; }
        public int Length { get; set; }
        public string ReplacementText { get; set; }
    }

    public class ClangTidyDiagnostic
    {
        public string DiagnosticName { get; set; }
        public List<ClangTidyReplacement> Replacements { get; set; }
    }

    public class FixItList
    {
        public string MainSourceFile { get; set; }
        public List<ClangTidyDiagnostic> Diagnostics { get; set; }
    }

    public class ClangCheck
    {
        private static string ContentDirectory => Path.Combine(PackageManager.GetPackageDirectory("AvalonStudio.Toolchains.Clang"), "content");
        private static string BinDirectory => Path.Combine(ContentDirectory, "bin");
        private static string ClangCheckCommand => Path.Combine(BinDirectory, "clang-tidy") + Platform.ExecutableExtension;

        public static async Task RunAsync(IConsole console, IStandardProject mainProject)
        {
            await PackageManager.EnsurePackage("AvalonStudio.Toolchains.Clang", console);

            var errorList = IoC.Get<IErrorList>();
            var shell = IoC.Get<IShell>();

            await Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                var toRemove = errorList.Errors.Where(e => e.Model.Source == DiagnosticSource.StaticAnalysis).ToList();
                foreach (var error in toRemove)
                {
                    errorList.RemoveDiagnostic(error);
                }

                errorList.ClearFixits(d => d.Source == DiagnosticSource.StaticAnalysis);
            });

            Diagnostic previous = null;
            int noteIndex = 1;

            mainProject.VisitSourceFiles((masterProject, project, file) =>
            {
                var settings = project.GetGenericSettings<CodeAnalysisSettings>();

                if (SupportsFile(file) && settings.Enabled)
                {
                    console.WriteLine($"Running analysis on: {file.Location}");

                    var args = GetCompilerArguments(masterProject, project, file);

                    string disabledChecks = "";
                    if(file.Extension == ".c")
                    {
                        disabledChecks += ",-misc-unused-parameters";
                    }

                    PlatformSupport.ExecuteShellCommand(ClangCheckCommand, $"-checks=-*,clang-analyzer-* {file.Location} -- {args} -D__STDC__", (s, e) =>
                    {
                        console.WriteLine(e.Data);

                        if (e.Data != null)
                        {
                            try
                            {
                                var parts = e.Data.Replace(":\\", ";\\").Replace("::",";;").Split(':', System.StringSplitOptions.RemoveEmptyEntries);

                                if (parts.Length == 5)
                                {
                                    var diagnostic = new Diagnostic
                                    {
                                        Project = project,
                                        File = shell.CurrentSolution.FindFile(parts[0].Replace(";\\", ":\\")),
                                        Line = Convert.ToInt32(parts[1]),
                                        Column = Convert.ToInt32(parts[2]),
                                        Spelling = parts[4].Replace(";;", "::"),
                                        Source = DiagnosticSource.StaticAnalysis
                                    };

                                    if (diagnostic.File != null)
                                    {
                                        switch (parts[3].Trim())
                                        {
                                            case "warning":
                                                diagnostic.Level = DiagnosticLevel.Warning;
                                                break;

                                            case "error":
                                                diagnostic.Level = DiagnosticLevel.Error;
                                                break;

                                            case "note":
                                                diagnostic.Level = DiagnosticLevel.Note;
                                                break;

                                            case "fatal":
                                                diagnostic.Level = DiagnosticLevel.Fatal;
                                                break;

                                            case "ignored":
                                                diagnostic.Level = DiagnosticLevel.Ignored;
                                                break;

                                            default:
                                                throw new NotImplementedException();
                                        }

                                        if (diagnostic.Level == DiagnosticLevel.Note && previous != null)
                                        {
                                            previous.Children.Add(diagnostic);
                                            diagnostic.Spelling = $"({noteIndex}) {diagnostic.Spelling}";
                                            noteIndex++;
                                        }
                                        else
                                        {
                                            Dispatcher.UIThread.InvokeAsync(() =>
                                            {
                                                errorList.AddDiagnostic(new ErrorViewModel(diagnostic));
                                            });

                                            previous = diagnostic;

                                            noteIndex = 1;
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }
                        }

                    }, (s, e) =>
                    {

                    },
                    false, file.CurrentDirectory, false);

                    //if (System.IO.File.Exists(Platform.GetCodeAnalysisFile(file)))
                    //{
                    //    using (var fileStream = System.IO.File.OpenText(Platform.GetCodeAnalysisFile(file)))
                    //    {
                    //        var yaml = new YamlStream();

                    //        var deserializer = new DeserializerBuilder()
                    //                    .Build();


                    //        var fixits = deserializer.Deserialize<FixItList>(fileStream);

                    //        if (fixits.Diagnostics != null)
                    //        {
                    //            foreach (var diag in fixits.Diagnostics)
                    //            {
                    //                var fixit = new FixIt
                    //                {
                    //                    File = file
                    //                };
                                    
                    //                foreach (var replacement in diag.Replacements)
                    //                {
                    //                    fixit.AddReplacement(new Replacement
                    //                    {
                    //                        Project = project,
                    //                        File = shell.CurrentSolution.FindFile(replacement.FilePath),
                    //                        Length = replacement.Length,
                    //                        StartOffset = replacement.Offset,
                    //                        ReplacementText = replacement.ReplacementText,
                    //                        Source = DiagnosticSource.StaticAnalysis
                    //                    });
                    //                }

                    //                errorList.AddFixIt(fixit);
                    //            }
                    //        }
                    //    }
                    //}
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
