namespace AvalonStudio.Projects
{
    using System.Collections.Generic;

    public interface ISolution
    {        
        void AddProject(IProject project);

        IList<IProject> Projects { get; }

        string CurrentDirectory { get; }
    }
}
