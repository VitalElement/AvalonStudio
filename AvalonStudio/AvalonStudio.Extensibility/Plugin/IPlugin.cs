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
        Task InstallAsync(IConsole console);
    }
    
    public interface IPlugin : IExtension
    {
        string Name { get; }
        Version Version { get; }
        string Description { get; }
    }
}