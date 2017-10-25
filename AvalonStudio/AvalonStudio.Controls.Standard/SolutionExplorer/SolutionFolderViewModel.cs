namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia.Media;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;

    public class SolutionFolderViewModel : SolutionParentViewModel<ISolutionFolder>
    {
        private DrawingGroup _folderOpenIcon;
        private DrawingGroup _folderIcon;

        public SolutionFolderViewModel(ISolutionParentViewModel parent, ISolutionFolder folder) : base(parent, folder)
        {
            _folderIcon = "FolderIcon".GetIcon();
            _folderOpenIcon = "FolderOpenIcon".GetIcon();
        }

        public override DrawingGroup Icon => IsExpanded ? _folderOpenIcon : _folderIcon;
    }
}