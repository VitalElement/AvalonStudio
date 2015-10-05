namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.Models.Solutions;
    using System;

    public class ProjectFileViewModel : ProjectItemViewModel
    {
        public ProjectFileViewModel(ProjectFile model)
            : base(model)
        {
        }

        public override bool CanAcceptDrop(Type type)
        {
            return false;
        }

        public override void Drop(ProjectItemViewModel item)
        {

        }

        public ProjectFile Model
        {
            get
            {
                return BaseModel as ProjectFile;
            }
        }
    }
}
