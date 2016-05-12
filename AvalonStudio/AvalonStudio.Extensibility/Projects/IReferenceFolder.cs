namespace AvalonStudio.Projects
{
    using System.Collections.ObjectModel;

    public interface IReferenceFolder : IProjectItem
    {
        ObservableCollection<IProject> References { get; }
    }
}
