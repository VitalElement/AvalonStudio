namespace AvalonStudio.Projects
{
    using System.Collections.Generic;

    public interface ISolution
    {        
        IProject StartupProject { get; }

        IProject AddProject(IProject project);

        IList<IProject> Projects { get; }

        string CurrentDirectory { get; }
    }
}
