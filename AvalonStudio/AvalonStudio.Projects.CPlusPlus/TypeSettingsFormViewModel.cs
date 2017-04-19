using AvalonStudio.MVVM;
using AvalonStudio.Projects.Standard;
using ReactiveUI;
using System;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class TypeSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
    {
        private string projectType;

        public TypeSettingsFormViewModel(CPlusPlusProject project) : base("Type", project)
        {
            projectType = project.Type.ToString();
        }

        public string[] ProjectTypeOptions
        {
            get { return Enum.GetNames(typeof(ProjectType)); }
        }

        public string ProjectType
        {
            get
            {
                return projectType;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref projectType, value);
                Save();
            }
        }

        public void Save()
        {
            Model.Type = (ProjectType)Enum.Parse(typeof(ProjectType), projectType);
            Model.Save();
        }
    }
}