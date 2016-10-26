using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.TypeScript
{
    public class TypeScriptToolchain : IToolChain
    {
        public string Description => "TypeScript Toolchain";

        public IList<string> Includes => new List<string>(); //None for now

        public string Name => "TypeScript";

        public Version Version => new Version(0, 1, 1, 0);

        public void Activation()
        {
            throw new NotImplementedException();
        }

        public void BeforeActivation()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Build(IConsole console, IProject project, string label = "", IEnumerable<string> definitions = null)
        {
            throw new NotImplementedException();
        }

        public bool CanHandle(IProject project)
        {
            throw new NotImplementedException();
        }

        public Task Clean(IConsole console, IProject project)
        {
            throw new NotImplementedException();
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