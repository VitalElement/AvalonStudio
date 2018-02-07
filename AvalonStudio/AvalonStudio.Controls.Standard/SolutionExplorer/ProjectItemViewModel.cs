namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia.Media;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;

    public abstract class ProjectItemViewModel : ViewModel
    {
        public abstract DrawingGroup Icon { get; }

        public virtual bool IsExpanded { get; set; } = false;

        public static ProjectItemViewModel Create(IProjectItem item)
        {
            ProjectItemViewModel result = null;

            if (item is IProjectFolder)
            {
                result = new ProjectFolderViewModel(item as IProjectFolder);
            }
            else if (item is ISourceFile)
            {
                result = new SourceFileViewModel(item as ISourceFile);
            }
            else if (item is IReferenceFolder)
            {
                result = new ReferenceFolderViewModel(item as IReferenceFolder);
            }

            return result;
        }
    }
}