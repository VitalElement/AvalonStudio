namespace AvalonStudio.Projects.CPlusPlus
{
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ReferenceViewModel : ViewModel<IProject>
    {
        private IProject referencer;

        public ReferenceViewModel(IProject referencer, IProject referencee) : base(referencee)
        {
            this.referencer = referencer;
            name = referencee.Name;
            isReferenced = referencer.References.Contains(referencee);
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }

        private bool isReferenced;
        public bool IsReferenced
        {
            get { return isReferenced; }
            set
            {
                if (isReferenced != value)
                {
                    isReferenced = value;

                    if (value)
                    {
                        referencer.References.Add(Model);
                    }
                    else
                    {
                        referencer.References.Remove(Model);
                    }

                    referencer.Save();
                }
            }
        }
    }

    public class ReferenceSettingsFormViewModel : ViewModel<CPlusPlusProject>
    {
        public ReferenceSettingsFormViewModel(CPlusPlusProject model) : base(model)
        {
            projects = new ObservableCollection<ReferenceViewModel>();

            foreach(var project in model.Solution.Projects)
            {
                if(project != model)
                {
                    projects.Add(new ReferenceViewModel(model, project));
                }
            }
        }

        private ObservableCollection<ReferenceViewModel> projects;
        public ObservableCollection<ReferenceViewModel> Projects
        {
            get { return projects; }
            set { this.RaiseAndSetIfChanged(ref projects, value); }
        }

    }
}
