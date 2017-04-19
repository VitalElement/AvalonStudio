using System.Collections.ObjectModel;

namespace AvalonStudio.Projects.Standard
{
    public class ReferenceFolder : IReferenceFolder
    {
        public ReferenceFolder(IProject project)
        {
            References = project.References;

            Parent = project;
            Project = project;
        }

        public string Name
        {
            get { return "References"; }
        }

        public ObservableCollection<IProject> References { get; set; }

        public IProject Project { get; set; }

        public IProjectFolder Parent { get; set; }

        public int CompareTo(IProjectItem other)
        {
            return this.CompareProjectItems(other);
        }
    }
}