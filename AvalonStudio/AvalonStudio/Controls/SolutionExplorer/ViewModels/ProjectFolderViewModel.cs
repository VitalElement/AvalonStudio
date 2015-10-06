namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.Models.Solutions;
    using System;

    class ProjectFolderViewModel : ProjectParentViewModel<ProjectFolder>
    {
        public ProjectFolderViewModel(ProjectFolder model)
            : base(model)
        {

        }

        public static ProjectFolderViewModel Create(ProjectFolder model)
        {
            return new ProjectFolderViewModel(model);
        }

        public override bool CanAcceptDrop(Type type)
        {
            bool result = false;

            if (type == typeof(ProjectFileViewModel))
            {
                result = true;
            }

            return result;
        }

        public override void Drop(ProjectItemViewModel item)
        {
            if (item is ProjectFileViewModel)
            {
                var file = item as ProjectFileViewModel;

                file.Model.MoveTo(this.Model);
            }
        }

        private ProjectFolder model;
        public ProjectFolder Model
        {
            get { return model; }
            set { model = value; }
        }        
    }
}
