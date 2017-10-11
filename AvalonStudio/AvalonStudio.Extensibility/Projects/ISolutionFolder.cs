using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public interface ISolutionFolder
    {
        ObservableCollection<ISolutionItem> Items { get; }
    }
}
