using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Avalonia.Controls;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Projects;
using AvalonStudio.Utils;

namespace AvalonStudio.Toolchains
{
    public interface IGDBToolchain
    {
        string GDBExecutable { get; }
    }

    [InheritedExport(typeof (IToolChain))]
	public interface IToolChain : IPlugin
	{
		IList<string> Includes { get; }
		Task<bool> Build(IConsole console, IProject project, string label = "");

		Task Clean(IConsole console, IProject project);

		UserControl GetSettingsControl(IProject project);

		IList<object> GetConfigurationPages(IProject project);

		void ProvisionSettings(IProject project);

		bool CanHandle(IProject project);
	}
}