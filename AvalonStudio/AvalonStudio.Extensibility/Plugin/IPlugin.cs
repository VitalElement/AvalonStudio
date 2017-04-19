using System;

namespace AvalonStudio.Extensibility.Plugin
{
    //[InheritedExport(typeof (IExtension))]
    public interface IExtension
    {
        void BeforeActivation();

        void Activation();
    }

    //[InheritedExport(typeof (IPlugin))]
    public interface IPlugin : IExtension
    {
        string Name { get; }
        Version Version { get; }
        string Description { get; }
    }
}