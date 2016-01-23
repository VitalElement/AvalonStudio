namespace AvalonStudio.Projects.VEBuild
{
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects.Standard;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    public class TypeSettingsFormViewModel : ViewModel<IProject>
    {
        public TypeSettingsFormViewModel(IProject project) : base (project)
        {
            try
            {
                projectType = project.Settings.ProjectType?.ToString();
            }
            catch (Exception e)
            {

            }
        }

        public void Save ()
        {
            Model.Settings.ProjectType =  (ProjectType)Enum.Parse(typeof(ProjectType), projectType);
            Model.Save();
        }

        public string[] ProjectTypeOptions
        {
            get
            {
                return Enum.GetNames(typeof(ProjectType));
            }
        }

        private string projectType;
        public string ProjectType
        {
            get { return projectType; }
            set { this.RaiseAndSetIfChanged(ref projectType, value); Save(); }
        }

    }
}
