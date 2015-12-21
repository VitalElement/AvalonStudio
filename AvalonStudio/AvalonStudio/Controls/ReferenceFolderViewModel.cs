namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.MVVM;
    using Projects;
    using ReactiveUI;
    using System.Collections.ObjectModel;

    public class ReferenceFolderViewModel : ViewModel
    {
        public ReferenceFolderViewModel (IProject project)
        {
            references = new ObservableCollection<ReferenceViewModel>();

            foreach (var reference in project.References)
            {
                references.Add(new ReferenceViewModel(reference));
            }
        }
       
        public string Title
        {
            get { return "References"; }
        }


        private ObservableCollection<ReferenceViewModel> references;
        public ObservableCollection<ReferenceViewModel> References
        {
            get { return references; }
            set { this.RaiseAndSetIfChanged(ref references, value); }
        }

    }
}
