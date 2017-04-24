using AvalonStudio.Utils;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Plugin
{
    //[InheritedExport(typeof (IExtension))]
    public interface IExtension
    {
        void BeforeActivation();

        void Activation();
    }

    public interface IInstallable
    {
        Task InstallAsync(IConsole console);
    }

    //[InheritedExport(typeof (IPlugin))]
    public interface IPlugin : IExtension
    {
        string Name { get; }
        Version Version { get; }
        string Description { get; }
    }
}