namespace AvalonStudio.Toolchains
{
    using Extensibility.Plugin;
    using Avalonia.Controls;
    using Projects;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Utils;

    [InheritedExport(typeof(IToolChain))]
    public interface IToolChain : IPlugin
    {
        Task<bool> Build(IConsole console, IProject project, string label = "");

        Task Clean(IConsole console, IProject project);        

        IList<string> Includes { get; }

        UserControl GetSettingsControl(IProject project);

        IList<object> GetConfigurationPages(IProject project);

        void ProvisionSettings(IProject project);

        bool CanHandle(IProject project);
    }
}

    
