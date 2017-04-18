namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using AvalonStudio.Extensibility;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using AvalonStudio.Shell;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;

    internal class ProjectFolderViewModel : ProjectItemViewModel<IProjectFolder>
    {
        private readonly IShell shell;

        public ProjectFolderViewModel(IProjectFolder model)
            : base(model)
        {
            shell = IoC.Get<IShell>();

            Items = new ObservableCollection<ProjectItemViewModel>();
            Items.BindCollections(model.Items, p => { return Create(p); }, (pivm, p) => pivm.Model == p);

            NewItemCommand = ReactiveCommand.Create();
            NewItemCommand.Subscribe(_ =>
            {
                shell.ModalDialog = new NewItemDialogViewModel(model);
                shell.ModalDialog.ShowDialog();
            });

            RemoveCommand = ReactiveCommand.Create();
            RemoveCommand.Subscribe(_ => { model.Project.ExcludeFolder(model); });
        }

        public ObservableCollection<ProjectItemViewModel> Items { get; }

        public ReactiveCommand<object> NewItemCommand { get; }
        public ReactiveCommand<object> RemoveCommand { get; }

        public static ProjectFolderViewModel Create(IProjectFolder model)
        {
            return new ProjectFolderViewModel(model);
        }
    }
}