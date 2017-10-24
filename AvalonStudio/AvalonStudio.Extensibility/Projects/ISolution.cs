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

        void UpdateItem(ISolutionItem item);

        T AddItem<T>(T item, ISolutionFolder parent = null) where T : ISolutionItem;

        void RemoveItem(ISolutionItem item);

        ISourceFile FindFile(string path);

        IProject FindProject(string name);

        void Save();
    }
}