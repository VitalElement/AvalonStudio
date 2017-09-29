namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using AvalonStudio.Extensibility;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using AvalonStudio.Shell;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;
    using Avalonia.Media;

    internal class ProjectFolderViewModel : ProjectItemViewModel<IProjectFolder>
    {
        private readonly IShell shell;

        private DrawingGroup _folderOpenIcon;
        private DrawingGroup _folderIcon;

        public ProjectFolderViewModel(IProjectFolder model)
            : base(model)
        {
            shell = IoC.Get<IShell>();

            Items = new ObservableCollection<ProjectItemViewModel>();
            Items.BindCollections(model.Items, p => { return Create(p); }, (pivm, p) => pivm.Model == p);

            NewItemCommand = ReactiveCommand.Create(() =>
            {
                shell.ModalDialog = new NewItemDialogViewModel(model);
                shell.ModalDialog.ShowDialog();
            });

            RemoveCommand = ReactiveCommand.Create(() => model.Project.ExcludeFolder(model));

            _folderIcon = "FolderIcon".GetIcon();
            _folderOpenIcon = "FolderOpenIcon".GetIcon();
        }

        public ObservableCollection<ProjectItemViewModel> Items { get; }

        public ReactiveCommand NewItemCommand { get; }
        public ReactiveCommand RemoveCommand { get; }

        private bool isExpanded;

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                this.RaisePropertyChanged(nameof(Icon));
            }
        }

        public override DrawingGroup Icon => IsExpanded ? _folderOpenIcon : _folderIcon;

        public static ProjectFolderViewModel Create(IProjectFolder model)
        {
            return new ProjectFolderViewModel(model);
        }
    }
}