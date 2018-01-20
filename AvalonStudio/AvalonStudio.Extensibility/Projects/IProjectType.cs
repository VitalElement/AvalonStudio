namespace AvalonStudio.Projects
{
    using AvalonStudio.Extensibility.Plugin;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProjectType : IExtension
    {
        Guid ProjectTypeId { get; }
        List<string> Extensions { get; }
        string Description { get; }

        Task<IProject> LoadAsync(ISolution solution, string filePath);
    }
}