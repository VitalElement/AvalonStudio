using System;
using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public class SolutionFolder : ISolutionFolder
    {
        public SolutionFolder(string name)
        {
            Name = name;
            Items = new ObservableCollection<ISolutionItem>();
        }

        public static SolutionFolder Create(string name)
        {
            return new SolutionFolder(name);
        }

        public ObservableCollection<ISolutionItem> Items { get; }

        public ISolution Solution { get; set; }

        public ISolutionFolder Parent { get; set; }

        public string Name { get; private set; }

        public Guid Id { get; set; }

        public void VisitChildren(Action<ISolutionItem> visitor)
        {
            foreach (var child in Items)
            {
                if (child is ISolutionFolder folder)
                {
                    folder.VisitChildren(visitor);
                }

                visitor(child);
            }
        }

        public int CompareTo(ISolutionItem other)
        {
            return this.DefaultCompareTo(other);
        }
    }
}
