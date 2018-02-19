namespace AvalonStudio.Toolchains.MSBuild
{
    using AvalonStudio.CommandLineTools;
    using AvalonStudio.GlobalSettings;
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using AvalonStudio.Projects.OmniSharp;
    using AvalonStudio.Projects.OmniSharp.DotnetCli;
    using AvalonStudio.Utils;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    public class MSBuildToolchain : IToolChain
    {
        public string BinDirectory => Path.Combine(Platform.ReposDirectory, "AvalonStudio.Languages.CSharp", "coreclr");

        public string Description => "Toolchain for MSBuild 15 (Dotnet Core Support)";

        public string Name => "MSBuild Toolchain";

        public Version Version => new Version(0, 0, 0);

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        private async Task<bool> BuildImpl (IConsole console, IProject project, List<IProject> builtList, string label = "", IEnumerable<string> definitions = null)
        {
            var netProject = project as OmniSharpProject;

            if (netProject.RestoreRequired)
            {
                var result = await netProject.Restore(console);

                if (!result)
                {
                    return false;
                }

                netProject.MarkRestored();
            }

            foreach (var reference in project.References)
            {
                if (!await BuildImpl(console, reference, builtList, label, definitions))
                {
                    return false;
                }
            }

            bool requiresBuild = false;

            if (!builtList.Contains(project))
            {
                if (project.Executable != null)
                {
                    if (!File.Exists(project.Executable))
                    {
                        requiresBuild = true;
                    }
                    else
                    {
                        var lastBuildDate = File.GetLastWriteTime(project.Executable);

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

                if (requiresBuild)
                {
                    return await Task.Factory.StartNew(() =>
                    {
                        var exitCode = PlatformSupport.ExecuteShellCommand(DotNetCliService.Instance.Info.Executable, $"msbuild {Path.GetFileName(project.Location)} /p:BuildProjectReferences=false /v:minimal", (s, e) =>
                        {
                            console.WriteLine(e.Data);
                        }, (s, e) =>
                        {
                            if (e.Data != null)
                            {
                                console.WriteLine();
                                console.WriteLine(e.Data);
                            }
                        },
                        false, project.CurrentDirectory, false);

                        builtList.Add(project);

                        return exitCode == 0;
                    });
                }
                else
                {
                    console.WriteLine($"[Skipped] {project.Name} -> {project.Executable}");
                    builtList.Add(project);
                }
            }

            return true;
        }

        public async Task<bool> Build(IConsole console, IProject project, string label = "", IEnumerable<string> definitions = null)
        {
            var builtList = new List<IProject>();

            if(await BuildImpl(console, project, builtList, label, definitions))
            {
                console.WriteLine("Build Successful");
                return true;
            }
            else
            {
                console.WriteLine("Build Failed");
                return false;
            }
        }

        public bool CanHandle(IProject project)
        {
            return project is OmniSharpProject;
        }

        public async Task Clean(IConsole console, IProject project)
        {
            await Task.Factory.StartNew(() =>
            {
                var settings = Settings.GetSettings<DotNetToolchainSettings>();

                if (string.IsNullOrEmpty(settings.DotNetPath))
                {
                    console.WriteLine("Please configure the location of the dotnet runtime and sdk.");
                    return;
                }

                var exitCode = PlatformSupport.ExecuteShellCommand(settings.DotNetPath, "clean", (s, e) => console.WriteLine(e.Data), (s, e) =>
                {
                    if (e.Data != null)
                    {
                        console.WriteLine();
                        console.WriteLine(e.Data);
                    }
                },
                false, project.CurrentDirectory, false);
            });
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