using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Projects
{
    public interface ISolution : ISolutionFolder
    {
        string Location { get; }

        IProject StartupProject { get; set; }

        IEnumerable<IProject> Projects { get; }

        string CurrentDirectory { get; }

        void UpdateItem(ISolutionItem item);

        T AddItem<T>(T item, Guid? itemGuid = null, ISolutionFolder parent = null) where T : ISolutionItem;

        void RemoveItem(ISolutionItem item);

        ISourceFile FindFile(string path);

        IProject FindProject(string name);

        IProject FindProjectByPath(string path);

        Task RestoreSolutionAsync();

        Task LoadSolutionAsync();

        Task LoadProjectsAsync();

        Task UnloadSolutionAsync();

        Task UnloadProjectsAsync();

        void Save();
    }
}