using System;
using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public interface ISolutionFolder : ISolutionItem
    {
        void VisitChildren(Action<ISolutionItem> visitor);

        ObservableCollection<ISolutionItem> Items { get; }
    }
}
