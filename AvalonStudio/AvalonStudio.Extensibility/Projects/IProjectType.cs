namespace AvalonStudio.Projects
{
    using AvalonStudio.Extensibility.Plugin;
    using System.Collections.Generic;
    
    public interface IProjectType : IExtension
    {
        List<string> Extensions { get; }
        string Description { get; }
        
        IProject Load(ISolution solution, string filePath);
    }
}
