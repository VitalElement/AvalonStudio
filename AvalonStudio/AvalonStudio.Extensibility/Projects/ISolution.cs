using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public interface ISolution : ISolutionFolder
    {
        string Location { get; }

        IProject StartupProject { get; set; }

        ObservableCollection<IProject> Projects { get; }

        string CurrentDirectory { get; }

        void SetItemParent(ISolutionItem item, ISolutionFolder parent);

        IProject AddProject(IProject project);

        ISourceFile FindFile(string path);

        void RemoveProject(IProject project);

        void Save();
    }
}