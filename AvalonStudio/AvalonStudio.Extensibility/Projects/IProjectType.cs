namespace AvalonStudio.Projects
{
    using AvalonStudio.Extensibility.Plugin;
    using System;
    using System.Collections.Generic;

    public interface IProjectType : IExtension
    {
        Guid ProjectTypeId { get; }
        List<string> Extensions { get; }
        string Description { get; }

        IProject Load(string filePath);
    }
}