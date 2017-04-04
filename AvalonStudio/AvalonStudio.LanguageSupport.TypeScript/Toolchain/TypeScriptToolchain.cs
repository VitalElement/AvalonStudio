using AvalonStudio.Projects;
using AvalonStudio.LanguageSupport.TypeScript.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvalonStudio.CommandLineTools;

namespace AvalonStudio.LanguageSupport.TypeScript.Toolchain
{
    public class TypeScriptToolchain : IToolChain
    {
        /// <summary>
        /// Stub
        /// </summary>
        public IList<string> Includes => new List<string>();

        public string Name => "TypeScript";

        public string Description => "TypeScript Toolchain";

        public Version Version => new Version(0, 1, 1, 2);

        public void Activation()
        {
            //throw new NotImplementedException();
        }

        public void BeforeActivation()
        {
            //throw new NotImplementedException();
        }

        public async Task<bool> Build(IConsole console, IProject project, string label = "", IEnumerable<string> definitions = null)
        {
            console.WriteLine($"Build Started - {project.Name}");
            //Make sure tools are available
            if (!PlatformSupport.CheckExecutableAvailability("tsc")) //Check if TSC is available
            {
                //TypeScript compiler missing
                console.WriteLine("The TypeScript compiler `tsc` is not available on your path. Please install it with `npm install -g typescript`");
                //Check if they're also missing Node
                if (!PlatformSupport.CheckExecutableAvailability("node"))
                {
                    console.WriteLine("You seem to be missing Node.js. Please install Node.js and TypeScript globally.");
                }
                console.WriteLine("Build failed.");
                return false; //Fail build
            }
            var tscVersionResult = PlatformSupport.ExecuteShellCommand("tsc", "-v");
            //Run build
            console.WriteLine($"Using TypeScript compiler {tscVersionResult.Output}");

            console.WriteLine($"TypeScript compile started...");
            var tsCompileExitCode = PlatformSupport.ExecuteShellCommand("tsc", $"-p {project.CurrentDirectory}", (s, a) => console.WriteLine(a.Data));

            if (tsCompileExitCode != 0) //compile error
            {
                console.WriteLine($"Build completed with code {tsCompileExitCode}");
            }
            else
            {
                console.WriteLine("Build completed successfully.");
            }

            return tsCompileExitCode == 0;
        }

        public bool CanHandle(IProject project)
        {
            return (project is TypeScriptProject);
        }

        public async Task Clean(IConsole console, IProject project)
        {
            //throw new NotImplementedException();
            //Run Clean task
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
    }
}