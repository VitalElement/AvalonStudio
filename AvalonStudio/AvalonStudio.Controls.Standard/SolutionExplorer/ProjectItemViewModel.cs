namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia.Media;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using ReactiveUI;
    using System.Reactive;

    public abstract class ProjectItemViewModel : ViewModel
    {
        public ProjectItemViewModel()
        {
            RenameCommand = ReactiveCommand.Create(() =>
            {
                InEditMode = true;
            });
        }

        private bool _isInEditMode;

        public abstract DrawingGroup Icon { get; }

        public virtual bool IsExpanded { get; set; } = false;

        public abstract string Title { get; set; }

        public bool InEditMode
        {
            get { return _isInEditMode; }
            set { this.RaiseAndSetIfChanged(ref _isInEditMode, value); }
        }

        public ReactiveCommand<Unit, Unit> RenameCommand { get; }

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