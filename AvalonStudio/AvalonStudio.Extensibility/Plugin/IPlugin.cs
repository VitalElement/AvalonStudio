using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Plugin
{
    public interface IExtension
    {
        void BeforeActivation();

        void Activation();
    }

    public interface IInstallable
    {
        Task<bool> InstallAsync(IConsole console, IProject project);
    }

    public interface IPlugin : IExtension
    {
        string Name { get; }
        Version Version { get; }
        string Description { get; }
    }
}