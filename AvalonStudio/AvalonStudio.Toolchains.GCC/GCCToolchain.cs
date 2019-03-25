using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Languages;
using AvalonStudio.Packaging;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.Standard;
using AvalonStudio.Utils;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.GCC
{
    public abstract class GCCToolchain : StandardToolchain
    {
        protected virtual bool RunWithSystemPaths => false;

        public virtual string GDBExecutable => "gdb";

        public virtual string LibraryQueryCommand => CCExecutable;

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

        public virtual string CCExecutable => Path.Combine(BinDirectory, $"{CCPPrefix}{CCName}" + Platform.ExecutableExtension);

        public virtual string CPPExecutable => Path.Combine(BinDirectory, $"{CCPPrefix}{CCPPName}" + Platform.ExecutableExtension);

        public virtual string ARExecutable => Path.Combine(BinDirectory, $"{ARPrefix}{ARName}" + Platform.ExecutableExtension);

        public virtual string LDExecutable => Path.Combine(BinDirectory, $"{LDPrefix}{LDName}" + Platform.ExecutableExtension);

        public virtual string SizeExecutable => Path.Combine(BinDirectory, $"{SizePrefix}{SizeName}" + Platform.ExecutableExtension);

        public virtual string SysRoot { get; set; }

        public virtual string[] ExtraPaths => new string[0];

        [ImportingConstructor]
        public GCCToolchain(IStatusBar statusBar)
            : base(statusBar)
        {
        }

        public override IList<object> GetConfigurationPages(IProject project)
        {
            return new List<object>
            {
                new SysRootSettingsFormViewModel(project),
                new CompileSettingsFormViewModel(project),
                new LinkerSettingsFormViewModel(project)
            };
        }

        public override bool SupportsFile(ISourceFile file)
        {
            var result = false;

            switch (file.Extension.ToLower())
            {
                case ".cc":
                case ".cpp":
                case ".c":
                case ".s":
                    result = true;
                    break;
            }

            if (!result)
            {
                var settings = file.Project.GetToolchainSettingsIfExists<GccToolchainSettings>();

                if (settings != null)
                {
                    var extensions = settings.CompileSettings.CompileExtensions;

                    if (extensions.Select(ext => "." + ext.ToLower()).Contains(file.Extension.ToLower()))
                    {
                        result = true;
                    }

                    extensions = settings.CompileSettings.AssembleExtensions;

                    if (extensions.Select(ext => "." + ext.ToLower()).Contains(file.Extension.ToLower()))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        private List<string> _cToolchainIncludes;
        private List<string> _cppToolchainIncludes;

        public override IEnumerable<string> GetToolchainIncludes(ISourceFile file)
        {
            if(_cToolchainIncludes == null)
            {
                return new List<string>();
            }

            if (file == null)
            {
                return _cToolchainIncludes;
            }

            switch (file.Extension.ToLower())
            {
                case ".cpp":
                case ".hpp":
                    return _cppToolchainIncludes;

                default:
                    return _cToolchainIncludes;
            }
        }

        private bool CheckFile(IConsole console, string file)
        {
            bool result = true;

            if (Platform.PlatformIdentifier != PlatformID.Unix && !PlatformSupport.CheckExecutableAvailability(file))
            {
                console.WriteLine("Unable to find tool (" + file + ") Please check project toolchain settings.");
                result = false;
            }

            return result;
        }

        public override bool ValidateToolchainExecutables(IConsole console)
        {
            bool result = true;

            if(string.IsNullOrEmpty(CCExecutable) || string.IsNullOrEmpty(CPPExecutable) || string.IsNullOrEmpty(ARExecutable) || string.IsNullOrEmpty(LDExecutable) || string.IsNullOrEmpty(SizeExecutable))
            {
                return false;
            }

            result = CheckFile(console, CCExecutable) && CheckFile(console, CPPExecutable) &&
            CheckFile(console, ARExecutable) && CheckFile(console, LDExecutable) &&
            CheckFile(console, SizeExecutable);

            return result;
        }

        private static readonly Regex errorRegex = new Regex((@"(?=.*(?:error|warning|line).*)(?<file>((?:[a-zA-Z]\:){0,1}(?:[\\\/][\w. ]+){1,})).*?(?<line>\d+).*?(?<column>\d+)(?::\s+)(?<type>warning|error)(?::\s+)(?<message>.*)"), RegexOptions.Compiled);

        private void ParseOutputForErrors(IList<Diagnostic> diagnostics, ISourceFile file, string output)
        {
            if (string.IsNullOrWhiteSpace(output))
            {
                return;
            }

            var match = errorRegex.Match(output);

            var filename = match.Groups["file"].Value;
            var type = match.Groups["type"].Value;
            var lineText = match.Groups["line"].Value;
            var columnText = match.Groups["column"].Value;
            var code = match.Groups["code"].Value;
            var message = match.Groups["message"].Value;

            if (match.Success)
            {
                int.TryParse(lineText, out int line);
                int.TryParse(columnText, out int column);

                diagnostics.Add(new Diagnostic(0, 0, file.Project.Name,
                filename, line, message, code,
                type == "error" ? DiagnosticLevel.Error : DiagnosticLevel.Warning, DiagnosticCategory.Compiler, DiagnosticSourceKind.Build));
            }
        }

        public override CompileResult Compile(IConsole console, IStandardProject superProject, IStandardProject project, ISourceFile file, string outputFile)
        {
            var result = new CompileResult();

            var settings = superProject.GetToolchainSettingsIfExists<GccToolchainSettings>().CompileSettings;

            string commandName = file.Extension == ".cpp" ? CPPExecutable : CCExecutable;

            var fileArguments = string.Empty;

            if (file.Extension.ToLower() == ".cpp" || (settings != null && settings.CompileExtensions.Select(ext => "." + ext.ToLower()).Contains(file.Extension.ToLower())))
            {
                fileArguments = "-x c++ -fno-use-cxa-atexit";
            }

            if (file.Extension.ToLower() == ".s" || (settings != null && settings.AssembleExtensions.Select(ext => "." + ext.ToLower()).Contains(file.Extension.ToLower())))
            {
                fileArguments = "-x assembler-with-cpp";
            }

            var environment = superProject.GetEnvironmentVariables().AppendRange(Platform.EnvironmentVariables);

            var arguments = "";

            if (!string.IsNullOrWhiteSpace(SysRoot))
            {
                arguments = string.Format("{0} {1} {2} -o{3} -MMD -MP --sysroot=\"{4}\"", fileArguments, GetCompilerArguments(superProject, project, file), file.Location, outputFile, SysRoot).ExpandVariables(environment);
            }
            else
            {
                arguments = string.Format("{0} {1} {2} -o{3} -MMD -MP", fileArguments, GetCompilerArguments(superProject, project, file), file.Location, outputFile).ExpandVariables(environment);
            }

            result.ExitCode = PlatformSupport.ExecuteShellCommand(commandName, arguments, (s, e) => console.WriteLine(e.Data), (s, e) =>
            {
                if (e.Data != null)
                {
                    ParseOutputForErrors(result.Diagnostics, file, e.Data);
                    console.WriteLine();
                    console.WriteLine(e.Data);
                }
            },
            false, "", false, RunWithSystemPaths, ExtraPaths);

            if (Studio.DebugMode)
            {
                console.WriteLine(Path.GetFileNameWithoutExtension(commandName) + " " + arguments);
            }

            return result;
        }

        public override LinkResult Link(IConsole console, IStandardProject superProject, IStandardProject project, CompileResult assemblies, string outputPath)
        {
            var result = new LinkResult();

            string commandName = project.Type == ProjectType.StaticLibrary ? ARExecutable : LDExecutable;

            var objectArguments = string.Empty;
            foreach (var obj in assemblies.ObjectLocations)
            {
                objectArguments += project.Solution.CurrentDirectory.MakeRelativePath(obj).ToPlatformPath() + " ";
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

            var outputName = Path.GetFileName(outputPath);

            if (project.Type == ProjectType.StaticLibrary)
            {
                outputName = Path.GetFileName(outputPath);
            }

            var executable = Path.Combine(outputDir, outputName);

            var linkedLibraries = string.Empty;

            foreach (var libraryPath in project.StaticLibraries)
            {
                var relativePath = Path.GetDirectoryName(libraryPath);

                var libName = Path.GetFileNameWithoutExtension(libraryPath).Substring(3);

                linkedLibraries += string.Format("-L\"{0}\" -l{1} ", relativePath, libName);
            }

            string libraryPaths = string.Empty;

            var linkerScripts = string.Empty;

            if (project.Type == ProjectType.Executable)
            {
                var settings = project.GetToolchainSettings<GccToolchainSettings>();

                foreach (var libraryPath in settings.LinkSettings.LinkedLibraries)
                {
                    var path = project.Solution.CurrentDirectory.MakeRelativePath(Path.Combine(project.CurrentDirectory, Path.GetDirectoryName(libraryPath)));
                    libraryPaths += $"-Wl,--library-path={path.ToPlatformPath()} ";

                    var libName = Path.GetFileName(libraryPath);

                    linkedLibraries += string.Format($"-Wl,--library=:{libName} ");
                }

                foreach (var script in settings.LinkSettings.LinkerScripts)
                {
                    linkerScripts += $"-Wl,-T\"{project.Solution.CurrentDirectory.MakeRelativePath(Path.Combine(project.CurrentDirectory, script)).ToPlatformPath()}\" ";
                }

                foreach (var lib in settings.LinkSettings.SystemLibraries)
                {
                    linkedLibraries += $"-l{lib} ";
                }
            }

            foreach (var lib in project.BuiltinLibraries)
            {
                linkedLibraries += $"-l{lib} ";
            }

            linkedLibraries += GetBaseLibraryArguments(superProject);

            string arguments = string.Empty;

            if (project.Type == ProjectType.StaticLibrary)
            {
                arguments = string.Format("rvs {0} {1}", executable, objectArguments);
            }
            else
            {
                var environment = superProject.GetEnvironmentVariables().AppendRange(Platform.EnvironmentVariables);

                if(!string.IsNullOrWhiteSpace(SysRoot))
                {
                    arguments = string.Format("{0} {1} -o{2} {3} {4} -Wl,--start-group {5} {6} -Wl,--end-group --sysroot=\"{7}\"", GetLinkerArguments(superProject, project).ExpandVariables(environment), linkerScripts, executable, objectArguments, libraryPaths, linkedLibraries, libs, SysRoot);
                }
                else
                {
                    arguments = string.Format("{0} {1} -o{2} {3} {4} -Wl,--start-group {5} {6} -Wl,--end-group", GetLinkerArguments(superProject, project).ExpandVariables(environment), linkerScripts, executable, objectArguments, libraryPaths, linkedLibraries, libs);
                }

            }

            result.ExitCode = PlatformSupport.ExecuteShellCommand(commandName, arguments, (s, e) =>
            {
                if (e.Data != null)
                {
                    console.WriteLine(e.Data);
                }
            },
                (s, e) =>
            {
                if (e.Data != null && !e.Data.Contains("creating"))
                {
                    console.WriteLine(e.Data);
                }
            }, false, project.Solution.CurrentDirectory, false, RunWithSystemPaths, ExtraPaths);

            if (Studio.DebugMode)
            {
                console.WriteLine(Path.GetFileNameWithoutExtension(commandName) + " " + arguments);
            }

            if (result.ExitCode == 0)
            {
                result.Executable = executable;
            }

            return result;
        }

        private async Task<List<string>> CalculateToolchainIncludes(bool cpp)
        {
            bool foundListStart = false;

            var result = new List<string>();

            string args = cpp ? "-xc++" : "-E";

            var process = PlatformSupport.LaunchShellCommand("echo", $" | {LibraryQueryCommand} {args} -Wp,-v -", (s, e) => { }, (s, e) =>
            {
                if (e.Data != null)
                {
                    if (!foundListStart)
                    {
                        if (e.Data == "#include <...> search starts here:")
                        {
                            foundListStart = true;
                        }
                    }
                    else
                    {
                        if (e.Data == "End of search list.")
                        {
                            foundListStart = false;
                        }
                        else
                        {
                            result.Add(e.Data.NormalizePath());
                        }
                    }
                }
            },
             false, BinDirectory, true, RunWithSystemPaths);

            await process.WaitForExitAsync();

            return result;
        }

        private async Task InitialiseInbuiltLibraries()
        {
            if (_cppToolchainIncludes == null || _cToolchainIncludes == null)
            {
                _cppToolchainIncludes = await CalculateToolchainIncludes(true);
                _cToolchainIncludes = await CalculateToolchainIncludes(false);
            }
        }

        public override async Task<bool> InstallAsync(IConsole console, IProject project)
        {
            await InitialiseInbuiltLibraries();

            var settings = project.GetToolchainSettingsIfExists<GccToolchainSettings>();

            if(settings != null && !string.IsNullOrWhiteSpace(settings.SysRoot) && (settings.SysRoot.Contains('?') || settings.SysRoot.Contains('='))) 
            {
                SysRoot = await PackageManager.ResolvePackagePathAsync(settings.SysRoot, appendExecutableExtension: false, console: console);
            }
            else
            {
                SysRoot = "";
            }

            return true;
        }

        public override async Task BeforeBuild(IConsole console, IProject project)
        {
            await base.BeforeBuild(console, project);

            var process = PlatformSupport.LaunchShellCommand($"{CCExecutable}", "--version", (s, e) =>
            {
                if (e.Data != null)
                {
                    console.WriteLine(e.Data);
                }
            }, (s, e) => { },
             false, BinDirectory, false, RunWithSystemPaths);

            await process.WaitForExitAsync();

            console.WriteLine();
        }

        public override ProcessResult Size(IConsole console, IStandardProject project, LinkResult linkResult)
        {
            var result = new ProcessResult();

            result.ExitCode = PlatformSupport.ExecuteShellCommand(SizeExecutable, linkResult.Executable,
                (s, e) => console.WriteLine(e.Data),
                (s, e) => console.WriteLine(e.Data),
                false, string.Empty, false, RunWithSystemPaths);

            return result;
        }
    }
}