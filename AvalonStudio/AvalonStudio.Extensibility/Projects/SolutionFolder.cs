using System;
using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public class SolutionFolder : ISolutionFolder
    {
        public SolutionFolder(string guid, string name, ISolutionFolder parent)
        {
            Name = name;
            Parent = parent;
            Solution = parent.Solution;
            Id = Guid.Parse(guid);
            Items = new ObservableCollection<ISolutionItem>();
        }

        public ObservableCollection<ISolutionItem> Items { get; }

        public ISolution Solution { get; set; }

        public ISolutionFolder Parent { get; set; }

        public string Name { get; private set; }

        public Guid Id { get; set; }

        public int CompareTo(ISolutionItem other)
        {
            return Name.CompareTo(other.Name);
        }
    }
}
