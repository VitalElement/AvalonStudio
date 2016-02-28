namespace AvalonStudio.Toolchains
{
    using Extensibility.Plugin;
    using Perspex.Controls;
    using Projects;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Utils;

    [InheritedExport(typeof(IToolChain))]
    public interface IToolChain : IPlugin
    {
        Task<bool> Build(IConsole console, IProject project);

        Task Clean(IConsole console, IProject project);        

        IList<string> Includes { get; }

        UserControl GetSettingsControl(IProject project);

        IList<TabItem> GetConfigurationPages(IProject project);

        void ProvisionSettings(IProject project);

        bool CanHandle(IProject project);
    }
}

    
