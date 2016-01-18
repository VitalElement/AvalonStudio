using System;

namespace AvalonStudio.Extensibility.Plugin
{
    public interface IPlugin
    {
        string Name { get; }
        Version Version { get; }
        string Description { get; }
    }
}
