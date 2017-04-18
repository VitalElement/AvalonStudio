namespace AvalonStudio.Toolchains.MSBuild
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AvalonStudio.Projects;
    using AvalonStudio.Utils;
    using AvalonStudio.Projects.OmniSharp;
    using AvalonStudio.CommandLineTools;
    using AvalonStudio.Platforms;
    using System.IO;
    using AvalonStudio.GlobalSettings;

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

        public async Task<bool> Build(IConsole console, IProject project, string label = "", IEnumerable<string> definitions = null)
        {
            return await Task.Factory.StartNew(() =>
            {
                var settings = SettingsBase.GetSettings<DotNetToolchainSettings>();

                var exitCode = PlatformSupport.ExecuteShellCommand(settings.DotNetPath, "build", (s, e) =>
                {
                    console.WriteLine(e.Data);

                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        if (e.Data.StartsWith($"  {project.Name} -> "))
                        {
                            project.Executable = e.Data.Substring($"  {project.Name} -> ".Length);
                        }
                    }
                }, (s, e) =>
                {
                    if (e.Data != null)
                    {
                        console.WriteLine();
                        console.WriteLine(e.Data);
                    }
                },
                false, project.CurrentDirectory, false);

                return exitCode == 0;
            });
        }

        public bool CanHandle(IProject project)
        {
            return project is OmniSharpProject;
        }

        public async Task Clean(IConsole console, IProject project)
        {
            await Task.Factory.StartNew(() =>
            {
                var settings = SettingsBase.GetSettings<DotNetToolchainSettings>();

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

        public void ProvisionSettings(IProject project)
        {
        }
    }
}
