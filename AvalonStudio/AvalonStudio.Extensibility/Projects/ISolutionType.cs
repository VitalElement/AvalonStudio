namespace AvalonStudio.Projects
{
    using AvalonStudio.Extensibility.Plugin;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISolutionType : IExtension
    {
        List<string> Extensions { get; }
        string Description { get; }

        Task<ISolution> LoadAsync(string path);
    }
}