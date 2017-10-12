using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public interface ISolutionFolder : ISolutionItem
    {
        ObservableCollection<ISolutionItem> Items { get; }
    }
}
