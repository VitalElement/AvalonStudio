using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using Avalonia.Media;
using System.Collections.Generic;

namespace AvalonStudio.Studio
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
            var priorityCompare = Priority.CompareTo(other.Priority); // need to bold the matched text.

            if(priorityCompare == 0)
            {
                return Location.CompareTo(other.Location);
            }

            return priorityCompare;
        }

        private IReadOnlyList<FormattedTextStyleSpan> spans;

        public IReadOnlyList<FormattedTextStyleSpan> Spans
        {
            get { return spans; }
            set { this.RaiseAndSetIfChanged(ref spans, value); }
        }
    }
}