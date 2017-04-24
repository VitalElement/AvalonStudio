using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains
{
    public interface IToolChain : IPlugin, IInstallable
    {
        IEnumerable<string> GetToolchainIncludes(ISourceFile file);

        Task<bool> Build(IConsole console, IProject project, string label = "", IEnumerable<string> definitions = null);

        Task Clean(IConsole console, IProject project);

        IList<object> GetConfigurationPages(IProject project);

        bool CanHandle(IProject project);
    }
}