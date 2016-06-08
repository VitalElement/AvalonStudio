namespace AvalonStudio.Projects.CPlusPlus
{
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects.Standard;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;

    public class TypeSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
    {
        public TypeSettingsFormViewModel(CPlusPlusProject project) : base ("Type", project)
        {
            projectType = project.Type.ToString();
        }

        

        public void Save ()
        {
            Model.Type =  (ProjectType)Enum.Parse(typeof(ProjectType), projectType);
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
