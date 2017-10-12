using System;
using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public class SolutionFolder : ISolutionFolder
    {
        public SolutionFolder(Guid guid, string name, ISolutionFolder parent)
        {
            Name = name;
            Parent = parent;
            Solution = parent?.Solution;
            Id = guid;
            Items = new ObservableCollection<ISolutionItem>();
        }

        public static SolutionFolder Create(string name, ISolutionFolder parent = null)
        {
            return new SolutionFolder(Guid.NewGuid(), name, parent);
        }

        public ObservableCollection<ISolutionItem> Items { get; }

        public ISolution Solution { get; set; }

        public ISolutionFolder Parent { get; set; }

        public string Name { get; private set; }

        public Guid Id { get; set; }

        public int CompareTo(ISolutionItem other)
        {
            return this.DefaultCompareTo(other);
        }
    }
}
