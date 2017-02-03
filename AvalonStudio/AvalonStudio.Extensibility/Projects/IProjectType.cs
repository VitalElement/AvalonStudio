namespace AvalonStudio.Projects
{
    using System.Collections.Generic;
    using System.Composition;

    //[InheritedExport(typeof(IProjectType))]
    public interface IProjectType
    {
        List<string> Extensions { get; }
        string Description { get; }

        IProject Load(string path);
    }
}
