namespace AvalonStudio.Projects
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface ISolution
    {   
        string Name { get; }     

        IProject StartupProject { get; set; }

        IProject AddProject(IProject project);

        ISourceFile FindFile(string path);

        void RemoveProject(IProject project);

        ObservableCollection<IProject> Projects { get; }

        string CurrentDirectory { get; }

        void Save();
    }
}
