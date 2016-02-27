namespace AvalonStudio.Controls.ViewModels
{
    using System;
    using Projects;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using MVVM;

    class ProjectFolderViewModel : ProjectItemViewModel<IProjectFolder>
    {
        public ProjectFolderViewModel(IProjectFolder model)
            : base(model)
        {
            Items = new ObservableCollection<ProjectItemViewModel>();
            Items.BindCollections(model.Items, (p) => { return ProjectItemViewModel.Create(p); }, (pivm, p) => pivm.Model == p);
            
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

        public ReactiveCommand<object> RemoveCommand { get; }
    }
}
