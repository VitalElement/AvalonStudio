using AvalonStudio.Projects;
using AvalonStudio.Projects.TypeScript;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.TypeScript
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
            //throw new NotImplementedException();
            console.WriteLine($"Build Started - {project.Name}");
            var buildProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "tsc",
                    RedirectStandardOutput = true,
                }
            };
            string buildProcessOutput = buildProcess.StandardOutput.ReadToEnd();
            //Run process and wait
            buildProcess.Start();
            buildProcess.WaitForExit();
            console.WriteLine(buildProcessOutput);
            console.WriteLine($"Build exited with code {buildProcess.ExitCode}");
            return buildProcess.ExitCode == 0;
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
            throw new NotImplementedException();
        }

        public void ProvisionSettings(IProject project)
        {
            throw new NotImplementedException();
        }
    }
}