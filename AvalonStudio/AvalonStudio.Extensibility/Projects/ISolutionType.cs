namespace AvalonStudio.Projects
{
    using System.Collections.Generic;
    using System.Composition;
    using System.Threading.Tasks;

    //[InheritedExport(typeof(ISolutionType))]
    public interface ISolutionType
    {
        List<string> Extensions { get; }
        string Description { get; }

        Task<ISolution> LoadAsync(string path);
    }
}
