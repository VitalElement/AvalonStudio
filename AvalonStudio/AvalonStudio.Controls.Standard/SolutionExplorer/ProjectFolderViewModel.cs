namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using MVVM;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;

    class ProjectFolderViewModel : ProjectItemViewModel<IProjectFolder>
    {
        public ProjectFolderViewModel(IProjectFolder model)
            : base(model)
        {
            Items = new ObservableCollection<ProjectItemViewModel>();
            Items.BindCollections(model.Items, (p) => { return ProjectItemViewModel.Create(p); }, (pivm, p) => pivm.Model == p);

            NewItemCommand = ReactiveCommand.Create();
            NewItemCommand.Subscribe(_ =>
            {
               // ShellViewModel.Instance.ModalDialog = new NewItemDialogViewModel(model);
                //ShellViewModel.Instance.ModalDialog.ShowDialog();
            });
            
            RemoveCommand = ReactiveCommand.Create();
            RemoveCommand.Subscribe(_ =>
            {
                model.Project.RemoveFolder(model);
            });
        }

        public static ProjectFolderViewModel Create(IProjectFolder model)
        {
            return new ProjectFolderViewModel(model);
        }

        public ObservableCollection<ProjectItemViewModel> Items { get; private set; }

        public ReactiveCommand<object> NewItemCommand { get; }
        public ReactiveCommand<object> RemoveCommand { get; }
    }
}
