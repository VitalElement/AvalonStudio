namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;

    public abstract class ProjectItemViewModel : ViewModel
    {
        public static ProjectItemViewModel Create(IProjectItem item)
        {
            ProjectItemViewModel result = null;

            if (item is IProjectFolder)
            {
                result = new ProjectFolderViewModel(item as IProjectFolder);
            }

            if (item is ISourceFile)
            {
                result = new SourceFileViewModel(item as ISourceFile);
            }

            if (item is IReferenceFolder)
            {
                result = new ReferenceFolderViewModel(item as IReferenceFolder);
            }

            return result;
        }
    }
}