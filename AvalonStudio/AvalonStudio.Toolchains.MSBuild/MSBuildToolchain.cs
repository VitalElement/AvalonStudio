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

    public class MSBuildToolchain : IToolChain
    {
        public string BinDirectory => Path.Combine(Platform.ReposDirectory, "AvalonStudio.Languages.CSharp", "coreclr");

        public string Description => "Toolchain for MSBuild 15 (Dotnet Core Support)";

        public string Name => "MSBuild Toolchain";

        public Version Version => new Version();

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
            string lastLine = string.Empty;

            var exitCode = PlatformSupport.ExecuteShellCommand(Path.Combine(BinDirectory, "dotnet" + Platform.ExecutableExtension), "build", (s, e) =>
            {
                console.WriteLine(e.Data);

                if (!string.IsNullOrEmpty(e.Data))
                {
                    lastLine = e.Data;
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

            if (exitCode == 0 && lastLine.StartsWith($"  {project.Name} -> "))
            {
                project.Executable = lastLine.Substring($"  {project.Name} -> ".Length);
            };

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
                var exitCode = PlatformSupport.ExecuteShellCommand(Path.Combine(BinDirectory, "dotnet" + Platform.ExecutableExtension), "clean", (s, e) => console.WriteLine(e.Data), (s, e) =>
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
            return new List<object>();
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
