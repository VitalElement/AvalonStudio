using AvalonStudio.MVVM;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ReferenceViewModel : ViewModel<IProject>
    {
        private bool isReferenced;

        private string name;
        private readonly IProject referencer;

        public ReferenceViewModel(IProject referencer, IProject referencee) : base(referencee)
        {
            this.referencer = referencer;
            name = referencee.Name;
            isReferenced = referencer.References.Contains(referencee);
        }

        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }

        public bool IsReferenced
        {
            get
            {
                return isReferenced;
            }
            set
            {
                if (isReferenced != value)
                {
                    isReferenced = value;

                    if (value)
                    {
                        referencer.AddReference(Model);
                    }
                    else
                    {
                        referencer.RemoveReference(Model);
                    }

                    referencer.Save();
                }
            }
        }
    }

    public class ReferenceSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
    {
        private ObservableCollection<ReferenceViewModel> projects;

        public ReferenceSettingsFormViewModel(CPlusPlusProject model) : base("References", model)
        {
            projects = new ObservableCollection<ReferenceViewModel>();

            foreach (var project in model.Solution.Projects)
            {
                if (project != model)
                {
                    projects.Add(new ReferenceViewModel(model, project));
                }
            }
        }

        public ObservableCollection<ReferenceViewModel> Projects
        {
            get { return projects; }
            set { this.RaiseAndSetIfChanged(ref projects, value); }
        }
    }
}