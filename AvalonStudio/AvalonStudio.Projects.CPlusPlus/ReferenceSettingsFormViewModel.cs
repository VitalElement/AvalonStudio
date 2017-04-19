using AvalonStudio.MVVM;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvalonStudio.Projects.CPlusPlus
{
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