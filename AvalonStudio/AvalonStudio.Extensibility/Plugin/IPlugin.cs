using System;
using System.ComponentModel.Composition;

namespace AvalonStudio.Extensibility.Plugin
{
    [InheritedExport(typeof(IPlugin))]
    public interface IPlugin
    {
        string Name { get; }
        Version Version { get; }
        string Description { get; }
        void BeforeActivation();
        void Activation();
    }
}
