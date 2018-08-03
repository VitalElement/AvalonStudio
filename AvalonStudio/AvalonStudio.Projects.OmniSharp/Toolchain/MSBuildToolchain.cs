using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Projects;
using AvalonStudio.Languages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Projects.OmniSharp;
using AvalonStudio.Projects.OmniSharp.Toolchain;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.MSBuild
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Flattens an object hierarchy.
        /// </summary>
        /// <param name="rootLevel">The root level in the hierarchy.</param>
        /// <param name="nextLevel">A function that returns the next level below a given item.</param>
        /// <returns><![CDATA[An IEnumerable<T> containing every item from every level in the hierarchy.]]></returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> rootLevel, Func<T, IEnumerable<T>> nextLevel)
        {
            HashSet<T> accumulation = new HashSet<T>();

            flattenLevel<T>(accumulation, rootLevel, nextLevel);

            foreach (var item in rootLevel)
            {
                accumulation.Add(item);
            }

            return accumulation;
        }

        /// <summary>
        /// Recursive helper method that traverses a hierarchy, accumulating items along the way.
        /// </summary>
        /// <param name="accumulation">A collection in which to accumulate items.</param>
        /// <param name="currentLevel">The current level we are traversing.</param>
        /// <param name="nextLevel">A function that returns the next level below a given item.</param>
        private static void flattenLevel<T>(HashSet<T> accumulation, IEnumerable<T> currentLevel, Func<T, IEnumerable<T>> nextLevel)
        {
            foreach (T item in currentLevel)
            {
                flattenLevel<T>(accumulation, nextLevel(item), nextLevel);

                foreach (var child in currentLevel)
                {
                    accumulation.Add(child);
                }
            }
        }
    }

    [ExportToolchain]
    [Shared]
    public class MSBuildToolchain : IToolchain
    {
        private static readonly Regex errorRegex = new Regex(@"(?<filename>[A-z0-9-_.]+)(?:\()(?<line>\d+)(?:,)(?<column>\d+)(?:\))(?::\s+)(?<type>warning|error)(?:\s)(?<code>[A-Z0-9]+)(?::\s)(?<message>.*)(?:\[)(?<project_file>[^;]*?)(?:\])");

        public string BinDirectory => Path.Combine(Platform.ReposDirectory, "AvalonStudio.Languages.CSharp", "coreclr");

        public string Description => "Toolchain for MSBuild 15 (Dotnet Core Support)";

        public string Name => "MSBuild Toolchain";

        public Version Version => new Version(0, 0, 0);

        private static bool RequiresBuilding(IProject project)
        {
            bool requiresBuild = false;

            if (project.Executable != null)
            {
                if (!File.Exists(project.Executable))
                {
                    requiresBuild = true;
                }
                else
                {
                    var lastBuildDate = File.GetLastWriteTime(project.Executable);

                    if (File.GetLastWriteTime(project.Location) > lastBuildDate)
                    {
                        requiresBuild = true;
                    }
                    else
                    {
                        foreach (var dependency in project.References)
                        {
                            // TODO implement public API analysis to determine if we need to rebuild or not.

                            if (File.GetLastWriteTime(dependency.Executable) > lastBuildDate)
                            {
                                requiresBuild = true;
                                break;
                            }
                        }

                        if (!requiresBuild)
                        {
                            foreach (var file in project.SourceFiles)
                            {
                                if (File.GetLastWriteTime(file.Location) > lastBuildDate)
                                {
                                    requiresBuild = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                // TODO unsure about this case, or if its possible?
                requiresBuild = true;
            }

            return requiresBuild;
        }

        private List<Task<(bool result, IProject project)>> QueueItems(List<IProject> toBuild, List<IProject> built, BuildRunner runner)
        {
            var tasks = new List<Task<(bool result, IProject project)>>();

            var toRemove = new List<IProject>();

            foreach (var item in toBuild)
            {
                bool canBuild = true;

                foreach (var dep in item.References.OfType<OmniSharpProject>())
                {
                    if (!built.Contains(dep))
                    {
                        canBuild = false;
                        break;
                    }
                }

                if (canBuild)
                {
                    toRemove.Add(item);
                    tasks.Add(runner.Queue(item));
                }
            }

            foreach (var item in toRemove)
            {
                toBuild.Remove(item);
            }

            return tasks;
        }

        private void ParseOutputForErrors(Dictionary<string, List<Diagnostic>> entries, string output)
        {
            if(string.IsNullOrWhiteSpace(output))
            {
                return;
            }

            var match = errorRegex.Match(output);

            var filename = match.Groups["filename"].Value;
            var type = match.Groups["type"].Value;
            var lineText = match.Groups["line"].Value;
            var columnText = match.Groups["column"].Value;
            var code = match.Groups["code"].Value;
            var message = match.Groups["message"].Value;
            var project_file = match.Groups["project_file"].Value;

            if(match.Success)
            {
                int.TryParse(lineText, out int line);
                int.TryParse(columnText, out int column);

                if (!entries.ContainsKey(filename + project_file))
                {
                    entries[filename + project_file] = new List<Diagnostic>();
                }

                entries[filename + project_file].Add(new Diagnostic(0, 0, project_file, filename, line, message, code,
                type == "warning" ? DiagnosticLevel.Warning : DiagnosticLevel.Error, DiagnosticCategory.Compiler,DiagnosticSourceKind.Build));
            }
        }

        public async Task<bool> BuildAsync(IConsole console, IProject project, string label = "", IEnumerable<string> definitions = null)
        {
            var diagnosticEntries = new Dictionary<string, List<Diagnostic>>();

            var errorList = IoC.Get<IErrorList>();
            errorList.Remove(this);

            var result = await Task.Factory.StartNew(() =>
            {
                var exitCode = PlatformSupport.ExecuteShellCommand(DotNetCliService.Instance.Info.Executable, $"build {Path.GetFileName(project.Location)}", (s, e) =>
                {
                    console.WriteLine(e.Data);
                    ParseOutputForErrors(diagnosticEntries, e.Data);
                }, (s, e) =>
                {
                    if (e.Data != null)
                    {
                        console.WriteLine();
                        console.WriteLine(e.Data);

                        ParseOutputForErrors(diagnosticEntries, e.Data);
                    }
                },
                false, project.CurrentDirectory, false);

                return exitCode == 0;
            });

            foreach(var key in diagnosticEntries.Keys)
            {
                errorList.Create(this, key, DiagnosticSourceKind.Build, diagnosticEntries[key].ToImmutableArray());
            }

            return result;
        }

        public bool CanHandle(IProject project)
        {
            return project is OmniSharpProject;
        }

        public async Task Clean(IConsole console, IProject project)
        {
            var errorList = IoC.Get<IErrorList>();
            errorList.Remove(this);

            await Task.Factory.StartNew(() =>
            {
                console.Write($"Cleaning Project: {project.Name}...");
                var exitCode = PlatformSupport.ExecuteShellCommand(DotNetCliService.Instance.DotNetPath, "clean /nologo", (s, e) => console.WriteLine(e.Data), (s, e) =>
                {
                    if (e.Data != null)
                    {
                        console.WriteLine();
                        console.WriteLine(e.Data);
                    }
                },
                false, project.CurrentDirectory, false);
            });

            console.WriteLine("Done.");
        }

        public IList<object> GetConfigurationPages(IProject project)
        {
            return new List<object>() { };
        }

        public IEnumerable<string> GetToolchainIncludes(ISourceFile file)
        {
            return new List<string>();
        }

        public Task<bool> InstallAsync(IConsole console, IProject project)
        {
            return Task.FromResult(true);
        }

        public void ProvisionSettings(IProject project)
        {
        }
    }
}