using System.Collections.ObjectModel;

namespace AvalonStudio.Projects
{
    public interface IReferenceFolder : IProjectItem
    {
        ObservableCollection<IProject> References { get; }
    }
}