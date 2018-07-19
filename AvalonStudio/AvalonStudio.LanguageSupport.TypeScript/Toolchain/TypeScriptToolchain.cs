using AvalonStudio.CommandLineTools;
using AvalonStudio.LanguageSupport.TypeScript.Projects;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;

namespace AvalonStudio.LanguageSupport.TypeScript.Toolchain
{
    [ExportToolchain]
    [Shared]
    public class TypeScriptToolchain : IToolchain
    {
        /// <summary>
        /// Stub
        /// </summary>
        public IList<string> Includes => new List<string>();

        public string Name => "TypeScript";

        public string Description => "TypeScript Toolchain";

        public Version Version => new Version(0, 1, 1, 2);

        public string BinDirectory => null;
        
        public Task<bool> BuildAsync(IConsole console, IProject project, string label = "", IEnumerable<string> definitions = null)
        {
            console.WriteLine($"Build Started - {project.Name}");
            //Make sure tools are available
            if (!PlatformSupport.CheckExecutableAvailability("tsc"))
            {
                //TypeScript compiler missing
                console.WriteLine("The TypeScript compiler `tsc` is not available on your path. Please install it with `npm install -g typescript`");
                //Check if they're also missing Node
                if (!PlatformSupport.CheckExecutableAvailability("node"))
                {
                    console.WriteLine("You seem to be missing Node.js. Please install Node.js and TypeScript globally.");
                }
                console.WriteLine("Build failed.");
                return Task.FromResult(false); //Fail build
            }
            var tscVersionResult = PlatformSupport.ExecuteShellCommand("tsc", "-v");
            //Run build
            console.WriteLine($"Using TypeScript compiler {tscVersionResult.Output}");

            console.WriteLine($"TypeScript compile started...");
            var compileExitCode = PlatformSupport.ExecuteShellCommand("tsc", $"-p {project.CurrentDirectory}", (s, a) => console.WriteLine(a.Data));

            if (compileExitCode != 0)
            {
                console.WriteLine($"Build completed with code {compileExitCode}");
            }
            else
            {
                console.WriteLine("Build completed successfully.");
            }

            return Task.FromResult(compileExitCode == 0);
        }

        public bool CanHandle(IProject project)
        {
            return project is TypeScriptProject;
        }

        public async Task Clean(IConsole console, IProject project)
        {
            //throw new NotImplementedException();
            //Run Clean task
            await Task.Delay(0);
        }

        public IList<object> GetConfigurationPages(IProject project)
        {
            //STUB!!
            return new List<object>();
        }

        public void ProvisionSettings(IProject project)
        {
            //STUB!!
        }

        public IEnumerable<string> GetToolchainIncludes(ISourceFile file)
        {
            //Irrelevant
            return new string[0];
        }

        public Task<bool> InstallAsync(IConsole console, IProject project)
        {
            return Task.FromResult(true);
        }
    }
}