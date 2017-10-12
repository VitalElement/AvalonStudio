using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public interface ISolution : ISolutionFolder
    {
        string Location { get; }

        IProject StartupProject { get; set; }

        IEnumerable<IProject> Projects { get; }

        string CurrentDirectory { get; }

        void SetItemParent(ISolutionItem item, ISolutionFolder parent);

        IProject AddProject(IProject project);

        void AddFolder(ISolutionFolder name);

        ISourceFile FindFile(string path);

        void RemoveProject(IProject project);

        void Save();
    }
}