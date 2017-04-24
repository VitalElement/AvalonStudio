using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public interface ISolution
    {
        string Name { get; }

        string Location { get; }

        IProject StartupProject { get; set; }

        ObservableCollection<IProject> Projects { get; }

        string CurrentDirectory { get; }

        IProject AddProject(IProject project);

        ISourceFile FindFile(string path);

        void RemoveProject(IProject project);

        void Save();
    }
}