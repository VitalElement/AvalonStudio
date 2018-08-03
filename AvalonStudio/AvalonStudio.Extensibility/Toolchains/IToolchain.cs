using AvalonStudio.Extensibility;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains
{
    public interface IToolchain : IInstallable, IExtension
    {
        IEnumerable<string> GetToolchainIncludes(ISourceFile file);

        Task<bool> BuildAsync(IConsole console, IProject project, string label = "", IEnumerable<string> definitions = null);

        Task Clean(IConsole console, IProject project);

        IList<object> GetConfigurationPages(IProject project);

        bool CanHandle(IProject project);

        string BinDirectory { get; }
    }
}