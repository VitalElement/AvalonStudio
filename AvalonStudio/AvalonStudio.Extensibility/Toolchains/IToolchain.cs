using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;
using Avalonia.Controls;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Projects;
using AvalonStudio.Utils;

namespace AvalonStudio.Toolchains
{
	////[InheritedExport(typeof (IToolChain))]
	public interface IToolChain : IPlugin
	{
        IEnumerable<string> GetToolchainIncludes(ISourceFile file);

        Task<bool> Build(IConsole console, IProject project, string label = "", IEnumerable<string> definitions = null);

		Task Clean(IConsole console, IProject project);

		IList<object> GetConfigurationPages(IProject project);

		void ProvisionSettings(IProject project);

		bool CanHandle(IProject project);
	}
}