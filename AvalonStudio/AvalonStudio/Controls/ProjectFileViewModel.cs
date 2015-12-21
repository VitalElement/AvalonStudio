namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.Models.Solutions;
    using System;

    public class ProjectFileViewModel : ProjectItemViewModel
    {
        public ProjectFileViewModel(ProjectFile model)            
        {
        }

        public override bool CanAcceptDrop(Type type)
        {
            return false;
        }

        public override void Drop(ProjectItemViewModel item)
        {

        }

        
        new public ProjectFile Model
        {
            get { return base.Model as ProjectFile; }
        }        
    }
}
