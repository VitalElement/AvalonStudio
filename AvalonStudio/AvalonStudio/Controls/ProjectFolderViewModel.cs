namespace AvalonStudio.Controls.ViewModels
{
    using Projects;
    using System.Collections.Generic;
    class ProjectFolderViewModel : ProjectItemViewModel<IProjectFolder>
    {
        public ProjectFolderViewModel(IProjectFolder model)
            : base(model)
        {
            Items = new List<ProjectItemViewModel>();

            foreach(var item in model.Items)
            {
                Items.Add(ProjectItemViewModel.Create(item));
            }
        }

        public static ProjectFolderViewModel Create(IProjectFolder model)
        {
            return new ProjectFolderViewModel(model);
        }

        public List<ProjectItemViewModel> Items { get; private set; }
    }
}
