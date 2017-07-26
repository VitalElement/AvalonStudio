using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using ReactiveUI;
using System;

namespace AvalonStudio.Controls
{
    public class SearchResultViewModel : ViewModel<ISourceFile>, IComparable<SearchResultViewModel>
    {
        public SearchResultViewModel(ISourceFile model) : base(model)
        {
        }

        public int Priority { get; set; }

        public string Title => Model.Name;

        public string Location => $"{Model.Project.Name}\\{Model.Project.Location.MakeRelativePath(Model.Location)}";

        public int CompareTo(SearchResultViewModel other)
        {
            var priorityCompare = other.Priority.CompareTo(Priority);

            if(priorityCompare == 0)
            {
                return Location.CompareTo(other.Location);
            }

            return priorityCompare;
        }
    }
}